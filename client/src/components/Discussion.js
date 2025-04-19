import React, { useState, useEffect } from "react";
import { useLocation, useNavigate, useParams } from "react-router-dom";
import proPicture from "../Images/blank-profile-picture.png";
import axios from "axios";
import { useToken } from "./TokenContext";
import { serverUrl, formatDate } from "../utils/global";

const Discussion = () => {
  const post = useLocation().state?.post;
  const navigate = useNavigate();
  const { id } = useParams();
  const { token, decoded } = useToken();
  const [inputValue, setInputValue] = useState("");
  const [comments, setComments] = useState([]);

  useEffect(() => {
    axios
      .get(`${serverUrl}posts/comments/${id}`, {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
      })
      .then((response) => {
        setComments(response.data);
      })
      .catch((error) => {
        if (!error.response) navigate("/posts");

        if (error.response.status === 404) console.log("no comments for this post yet.");
        else {
          alert("Something went wrong. Please try again.");
          navigate("/posts");
        }
      });
  }, []);

  const OnCommentClick = async () => {
    const requestBody = {
      postId: id,
      name: decoded.name,
      text: inputValue,
    };

    axios
      .post(`${serverUrl}posts/comment`, requestBody, {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
      })
      .then((response) => {
        setComments((comments) => [...comments, response.data]);
        setInputValue("");
      })
      .catch((error) => console.log(error));
  };

  return (
    <>
      {comments ? (
        <div className="container">
          <div className="post-card" style={{ marginTop: 50 }}>
            <div>
              <img alt="Profile" src={proPicture} style={{ height: 60, borderRadius: 50 }} />
              <h3>{post.name}</h3>
            </div>
            <div>
              <p>
                <b>{post.text}</b>
              </p>
              <small style={{ color: "gray" }}>Posted at {post.date ? formatDate(post.date) : formatDate(Date.now())}</small>
            </div>
          </div>
          <div className="post">
            <h3 className="form-title" style={{ textAlign: "center" }}>
              Leave Comment
            </h3>
            <hr />
            <input value={inputValue} onChange={(e) => setInputValue(e.target.value)} placeholder="Share your opionion?" />
            <button className="btn  btn-primary" style={{ position: "absolute", bottom: 0, left: 0 }} onClick={OnCommentClick}>
              Comment
            </button>
          </div>

          {comments.length < 1 ? (
            <p style={{ marginTop: 20, fontSize: 20 }}>No comments found</p>
          ) : (
            comments.map((comment, index) => (
              <div key={index} className="post-card">
                <div>
                  <img alt="Profile" title="Profile" src={proPicture} style={{ height: 60, borderRadius: 50 }} />
                  <h3>{comment.name}</h3>
                </div>
                <div>
                  <p>
                    <b>{comment.text}</b>
                  </p>
                  <small style={{ color: "gray" }}>Posted at {comment.date ? formatDate(comment.date) : formatDate(Date.now())}</small>
                </div>
              </div>
            ))
          )}
        </div>
      ) : (
        <div>Loading Posts</div>
      )}
    </>
  );
};

export default Discussion;
