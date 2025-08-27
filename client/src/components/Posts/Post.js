import { Avatar } from "@mui/material";
import { formatDate } from "../../utils/global";
import { useToken } from "../TokenContext";
import { Link } from "react-router-dom";
import { LikePost } from "../../utils/APIs/postService";
import { useState } from "react";
import defaultAvatar from "../../Images/blank-profile-picture.png";

const Post = ({ post }) => {
  const { token } = useToken();
  const [likes, setLikes] = useState(post?.likes ?? 0);

  const OnLikeClick = async (post) => {
    try {
      await LikePost(token, post.id);
      setLikes((prev) => prev + 1);
    } catch (error) {
      console.error("Like failed:", error);
    }
  };

  return (
    <div className="post-card">
      <div>
        <Avatar alt={post.name} src={post.avatarURL ?? defaultAvatar} sx={{ width: 50, height: 50 }} />
      </div>
      <div className="bg-[--post-color] p-5 my-2 w-[95%] rounded-xl">
        <div className="flex">
          <h3 className="text-[--primary-color]">{post.name}</h3>
          <small className="ml-5 text-gray-100 opacity-50">Posted at {post.date ? formatDate(post.date) : formatDate(Date.now())}</small>
        </div>

        <div>
          <p className="whitespace-pre-line text-white my-3">
            <b>{post.text}</b>
          </p>

          {post.imageURL && !post.imageURL.startsWith("blob:") && <img className="rounded-xl block w-full" src={post.imageURL} alt="Post" />}
          <div className="flex">
            <button
              type="button"
              className={`border-none btn flex justify-center text-[--primary-color] hover:bg-[--secondary-color] ${
                post.isLiked ? "bg-gray-800" : "bg-inherit"
              }`}
              onClick={(e) => {
                OnLikeClick(post, e.target);
              }}
            >
              <span className="pr-2">{likes === 0 ? "" : likes}</span>

              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="size-6">
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M21 8.25c0-2.485-2.099-4.5-4.688-4.5-1.935 0-3.597 1.126-4.312 2.733-.715-1.607-2.377-2.733-4.313-2.733C5.1 3.75 3 5.765 3 8.25c0 7.22 9 12 9 12s9-4.78 9-12Z"
                />
              </svg>
            </button>

            <Link
              to={`/home/discussion/${post.id}`}
              className="flex justify-center border-none btn bg-inherit text-[--primary-color]  hover:bg-[--secondary-color] justify-center"
            >
              <span className="pr-2">{post.comments === 0 ? "" : post.comments}</span>

              <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" strokeWidth={1.5} stroke="currentColor" className="size-6">
                <path
                  strokeLinecap="round"
                  strokeLinejoin="round"
                  d="M12 20.25c4.97 0 9-3.694 9-8.25s-4.03-8.25-9-8.25S3 7.444 3 12c0 2.104.859 4.023 2.273 5.48.432.447.74 1.04.586 1.641a4.483 4.483 0 0 1-.923 1.785A5.969 5.969 0 0 0 6 21c1.282 0 2.47-.402 3.445-1.087.81.22 1.668.337 2.555.337Z"
                />
              </svg>
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Post;
