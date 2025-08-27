import { useState, useEffect } from "react";
import { useToken } from "../TokenContext";
import Post from "./Post";
import { SavePost, GetAllComments, GetPostById } from "../../utils/APIs/postService";
import { useParams, useNavigate } from "react-router-dom";
import { getMyProfile } from "../../utils/APIs/profileService";

const Discussion = () => {
  const { token, decoded } = useToken();
  const [inputValue, setInputValue] = useState("");
  const [comments, setComments] = useState([]);
  const { postId } = useParams();
  const [post, setPost] = useState({});
  const [posting, setPosting] = useState(false);

  const navigate = useNavigate();

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
    setPosting(true);

    const fetchProfile = async () => {
      return await getMyProfile(token)
        .then((data) => data)
        .catch((error) => {
          if (error.response?.status === 404) {
            alert("You do not have a profile, please create one before posting.");
            navigate("/profile/edit");
          } else {
            alert("Failed to fetch profile. Please try again.");
            navigate("/login");
          }
        });
    };
    const profile = await fetchProfile();

    if (!profile) {
      setPosting(false);
      return;
    }

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
            avatarURL: response.avatarURL,
          },
        ]);
        setInputValue("");
      })
      .catch((error) => {
        alert(error.message);
      });

    setPosting(false);
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
            <button className="btn  btn-primary btn-comment  absolute bottom-0 right-0" onClick={OnCommentClick} disabled={posting}>
              {posting ? "Commenting..." : "Comment"}
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
