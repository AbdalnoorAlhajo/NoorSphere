import React, { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import proPicture from "../Images/blank-profile-picture.png";
import axios from "axios";
import { useToken } from "./TokenContext";
import { serverUrl, formatDate } from "../utils/global";

const Posts = () => {
  const [posts, setPosts] = useState([]);
  const [inputValue, setInputValue] = useState("");
  const { token, decoded } = useToken();

  const navigate = useNavigate();

  useEffect(() => {
    axios
      .get(`${serverUrl}posts/all`, {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
      })
      .then((response) => {
        setPosts(response.data);
      })
      .catch((error) => {
        if (error.response) {
          if (error.response.status === 404) alert("No post found. Create the first post.");
          else {
            alert("Something went wrong. Please try again.");
            navigate("/posts");
          }
        } else {
          alert("An unexpected error occurred. Please try again later.");
          navigate("/posts");
        }
      });
  }, []);

  const OnPostClick = () => {
    const requestBody = {
      name: decoded.name,
      text: inputValue,
    };

    axios
      .post(`${serverUrl}posts`, requestBody, {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
      })
      .then((response) => {
        console.log(response.data);

        setPosts((posts) => [
          ...posts,
          {
            user: response.data.user,
            name: response.data.name,
            text: response.data.text,
            date: response.data.date,
            id: response.data.id,
            likes: response.data.likes,
          },
        ]);

        setInputValue("");
      })
      .catch((error) => {
        if (error.response) {
          if (error.response.status === 404) alert("User does not exist or user does not have profile.");
          else if (error.response.status === 400) alert("Bad request. Please check your input.");
          else alert("Something went wrong. Please try again.");
        } else alert("An unexpected error occurred. Please try again later.");
      });
  };

  const OnLikeClick = async (post, button) => {
    axios
      .post(`${serverUrl}/posts/like/${post.id}`, null, {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
      })
      .then((response) => {
        post.likes = response.data;
        ChangeColorImmidatly(button);
      })
      .catch((error) => {
        console.error(error);
      });
  };

  const IsLiked = (post) => {
    return post.likes?.some((e) => e.user === decoded?.user?.id) || false;
  };

  const ChangeColorImmidatly = (button) => {
    button.classList.toggle("btn-dark");
    button.classList.toggle("btn-light");
  };

  return (
    <>
      {posts ? (
        <div className="container">
          <div className="post">
            <h3 className="form-title text-center">Create Post</h3>

            <hr />
            <input value={inputValue} onChange={(e) => setInputValue(e.target.value)} placeholder="What is in your mind?" />

            <button className="btn btn-primary absolute bottom-0 left-0" onClick={OnPostClick}>
              Post
            </button>
          </div>

          {posts.map((post, index) => (
            <div key={index} className="post-card">
              <div>
                <img className="w-20 rounded-full" src={proPicture} alt="Profile " title="Profile" />
                <h3>{post.name}</h3>
              </div>

              <div>
                <p>
                  <b>{post.text}</b>
                </p>

                <small className="mr-5 text-gray-600">Posted at {post.date ? formatDate(post.date) : formatDate(Date.now())}</small>

                <button
                  type="button"
                  className={IsLiked(post) ? "btn btn-dark" : "btn btn-light"}
                  onClick={(e) => {
                    OnLikeClick(post, e.target);
                  }}
                >
                  <i className="fas fa-thumbs-up" />
                  <span>{post.likes?.length ? ` ${post.likes.length}` : ""}</span>
                </button>

                <button type="button" className={"btn btn-light"} onClick={(e) => ChangeColorImmidatly(e.currentTarget)}>
                  <i className="fas fa-thumbs-down" />
                </button>

                <Link to={`discussion/${post.id}`} state={{ post }} className="btn btn-primary">
                  Discussion {post.comments?.length ? <span className="comment-count"> {post.comments.length}</span> : ""}
                </Link>
              </div>
            </div>
          ))}
        </div>
      ) : (
        <div>Loading Posts</div>
      )}
    </>
  );
};

export default Posts;
