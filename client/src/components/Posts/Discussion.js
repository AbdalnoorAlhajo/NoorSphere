import { useState, useEffect } from "react";
import { useToken } from "../TokenContext";
import Post from "./Post";
import { SavePost, GetAllComments, GetPostById } from "../../utils/APIs/postService";
import { useParams } from "react-router-dom";

const Discussion = () => {
  const { token, decoded } = useToken();
  const [inputValue, setInputValue] = useState("");
  const [comments, setComments] = useState([]);
  const { postId } = useParams();
  const [post, setPost] = useState({});

  useEffect(() => {
    const GetAllCommentsById = async () => {
      try {
        const response = await GetAllComments(token, postId);
        setComments(response);
      } catch (error) {
        setComments([]);
        console.log("Error in fetting comments", error.message);
        if (error === "No post found.") console.log("no comments for this post yet.");
      }
    };

    const FetchPostById = async () => {
      try {
        console.log("post id: ", postId);
        const response = await GetPostById(token, postId);
        setPost(response);
      } catch (error) {
        console.log("Error in fetting comments", error.message);
        if (error === "No post found. Create the first post.") console.log("no comments for this post yet.");
        else {
          alert("Something went wrong. Please try again.");
        }
      }
    };

    GetAllCommentsById();
    FetchPostById();
  }, [postId, token]);

  const OnCommentClick = async () => {
    const requestBody = {
      postId: post.id,
      name: decoded.name,
      text: inputValue,
    };

    SavePost(token, requestBody)
      .then((response) => {
        setComments((posts) => [
          ...posts,
          {
            user: response.user,
            name: response.name,
            text: response.text,
            imageURL: response.imageURL,
            date: response.date,
            id: response.id,
            likes: response.likes,
          },
        ]);
        setInputValue("");
      })
      .catch((error) => {
        alert(error.message);
      });

    // axios
    //   .post(`${serverUrl}posts/comment`, requestBody, {
    //     headers: {
    //       "Content-Type": "application/json",
    //       Authorization: "Bearer " + token,
    //     },
    //   })
    //   .then((response) => {
    //     setComments((comments) => [...comments, response.data]);
    //     setInputValue("");
    //   })
    //   .catch((error) => console.log(error));
  };

  return (
    <>
      <div className="container">
        <Post post={post} />

        <div className="post">
          <h3 className="form-title text-center">Leave Comment</h3>

          <input
            className="bg-inherit pb-6 text-xl outline-none  placeholder:text-[--primary-color] text-[--primary-color]"
            value={inputValue}
            onChange={(e) => setInputValue(e.target.value)}
            placeholder="Share your opionion?"
          />
          <div className="relative h-10 mt-2">
            <button className="btn  btn-primary" style={{ position: "absolute", bottom: 0, right: 0 }} onClick={OnCommentClick}>
              Comment
            </button>
          </div>
        </div>
      </div>
      {comments.length < 1 ? (
        <p className="text-[--primary-color] text-2xl my-10">No comments found</p>
      ) : (
        comments.map((comment) => <Post key={comment.id} post={comment} />)
      )}
    </>
  );
};

export default Discussion;
