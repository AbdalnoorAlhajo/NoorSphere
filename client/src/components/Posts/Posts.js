import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { Tab, Tabs } from "@mui/material";
import Post from "../Posts/Post";
import { GetAllPosts, SavePost, GetPostsByFollowedUsers, GeneratePostWithAI } from "../../utils/APIs/postService";
import { getMyProfile } from "../../utils/APIs/profileService";
import { UploadImageToCloudinary } from "../../utils/APIs/imageService";

const Posts = () => {
  const [posts, setPosts] = useState([]);
  const [inputValue, setInputValue] = useState("");
  const { token, decoded } = useToken();
  const [value, setValue] = useState("1");
  const [isLoading, setIsLoading] = useState(true);
  const [imageURL, setImageURL] = useState(null);
  const [imageFile, setImageFile] = useState(null);
  const [posting, setPosting] = useState(false);
  const [isGetMorePosts, setIsGetMorePosts] = useState(false);
  const [isThereMorePosts, setIsThereMorePosts] = useState(true);
  const [isOpen, setIsOpen] = useState(false);
  const [prompt, setPrompt] = useState("");
  const [isCreatingPost, setIsCreatingPost] = useState(false);
  const [LastPostId, setLastPostId] = useState(0);

  const navigate = useNavigate();

  const handleChange = (event, newValue) => {
    setValue(newValue);
    setIsThereMorePosts(true);
    setLastPostId(0);
  };

  const generatePost = async (post) => {
    setIsCreatingPost(true);
    try {
      const response = await GeneratePostWithAI(token, post);
      setInputValue(response);
      setIsOpen(false);
    } catch (error) {
      console.error("Error generating post:", error.response ? error.response.data : error.message);
      return null;
    } finally {
      setIsCreatingPost(false);
    }
  };

  useEffect(() => {
    setIsLoading(true);

    if (value === "1") {
      const fetchPosts = async () => {
        setIsGetMorePosts(true);
        try {
          const data = await GetAllPosts(token, LastPostId);

          if (data.length < 2) setIsThereMorePosts(false);

          LastPostId === 0 ? setPosts(data) : setPosts((posts) => [...posts, ...data]);
        } catch (error) {
          alert(error.message);
          if (error.message === "Unauthorized") {
            navigate("/login");
          }
        } finally {
          setIsLoading(false);
          setIsGetMorePosts(false);
        }
      };
      fetchPosts();
    } else if (value === "2") {
      const fetchPostsByFollowedUsers = async () => {
        try {
          const data = await GetPostsByFollowedUsers(token);
          setPosts(data);
        } catch (error) {
          if (error.message === "Unauthorized") {
            navigate("/login");
          }
          setPosts([]);
        } finally {
          setIsLoading(false);
        }
      };
      fetchPostsByFollowedUsers();
    }
  }, [token, navigate, value, LastPostId]);

  const OnPostClick = async () => {
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

    let cloudURL = null;
    try {
      if (imageFile) {
        cloudURL = await UploadImageToCloudinary(imageFile);
      }
    } catch (error) {
      alert("Image upload failed. Please try again.");
      setPosting(false);
      return;
    }

    const requestBody = {
      name: decoded.name,
      text: inputValue,
      imageURL: cloudURL,
    };

    SavePost(token, requestBody)
      .then((response) => {
        setPosts((posts) => [
          ...posts,
          {
            user: response.user,
            name: response.name,
            text: response.text,
            imageURL: response.imageURL,
            avatarURL: response.avatarURL,
            date: response.date,
            id: response.id,
            likes: response.likes,
          },
        ]);
        setInputValue("");
        setImageURL(null);
      })
      .catch((error) => {
        alert(error.message);
      });

    setPosting(false);
  };

  const GetMorePosts = () => {
    if (posts.length > 0) {
      setLastPostId(posts[posts.length - 1].id);
    }
  };

  if (isLoading) return <div className="text-white m-6">Loading...</div>;
  else
    return (
      <div className="container">
        <div className="mb-4">
          <Tabs value={value} onChange={handleChange} aria-label="Fillter posts">
            <Tab label="All" value="1" />
            <Tab label="Following" value="2" />
          </Tabs>
        </div>
        {value === "2" ? (
          ""
        ) : (
          <div className="post">
            <textarea
              value={inputValue}
              onChange={(e) => setInputValue(e.target.value)}
              placeholder="What is in your mind?"
              className="w-full h-full p-[10px] m-0 bg-inherit placeholder:text-[--primary-color] text-[--primary-color] rounded-[10px] border border-none resize-none"
            ></textarea>
            {imageURL && (
              <div className="mt-2">
                <img src={imageURL} alt="Preview" className="rounded-lg max-w-full h-auto" />
              </div>
            )}
            <div className="relative h-14">
              <div className="absolute bottom-2 left-2">
                {/* Adding image to post */}

                <input
                  type="file"
                  id="image-upload"
                  accept="image/*"
                  className="hidden"
                  onChange={(e) => {
                    const file = e.target.files[0];
                    if (file) {
                      const url = URL.createObjectURL(file);
                      setImageURL(url);
                      setImageFile(file);
                    }
                  }}
                />

                <label htmlFor="image-upload" title="Image Upload" className="cursor-pointer">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth={1.5}
                    stroke="currentColor"
                    className="stroke-[--primary-color] size-10"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="m2.25 15.75 5.159-5.159a2.25 2.25 0 0 1 3.182 0l5.159 5.159m-1.5-1.5 
         1.409-1.409a2.25 2.25 0 0 1 3.182 0l2.909 2.909m-18 3.75h16.5a1.5 
         1.5 0 0 0 1.5-1.5V6a1.5 1.5 0 0 0-1.5-1.5H3.75A1.5 1.5 0 0 
         0 2.25 6v12a1.5 1.5 0 0 0 1.5 1.5Zm10.5-11.25h.008v.008h-.008V8.25Zm.375 
         0a.375.375 0 1 1-.75 0 .375.375 0 0 1 .75 0Z"
                    />
                  </svg>
                </label>

                {/* Adding poll to post */}
                <button
                  onClick={() => alert("Poll feature is not implemented yet.")}
                  title="Poll"
                  className="ml-2 bg-inherit border-none cursor-pointer"
                >
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth={1.5}
                    stroke="currentColor"
                    className="stroke-[--primary-color] size-10"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M7.5 14.25v2.25m3-4.5v4.5m3-6.75v6.75m3-9v9M6 20.25h12A2.25 2.25 0 0 0 20.25 18V6A2.25 2.25 0 0 0 18 3.75H6A2.25 2.25 0 0 0 3.75 6v12A2.25 2.25 0 0 0 6 20.25Z"
                    />
                  </svg>
                </button>

                {/* Create post with AI*/}
                <button onClick={() => setIsOpen(true)} title="Create post with AI" className="ml-2 bg-inherit border-none cursor-pointer">
                  <svg
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                    strokeWidth={1.5}
                    stroke="currentColor"
                    className="stroke-[--primary-color] size-10"
                  >
                    <path
                      strokeLinecap="round"
                      strokeLinejoin="round"
                      d="M9.813 15.904 9 18.75l-.813-2.846a4.5 4.5 0 0 0-3.09-3.09L2.25 12l2.846-.813a4.5 4.5 0 0 0 3.09-3.09L9 5.25l.813 2.846a4.5 4.5 0 0 0 3.09 3.09L15.75 12l-2.846.813a4.5 4.5 0 0 0-3.09 3.09ZM18.259 8.715 18 9.75l-.259-1.035a3.375 3.375 0 0 0-2.455-2.456L14.25 6l1.036-.259a3.375 3.375 0 0 0 2.455-2.456L18 2.25l.259 1.035a3.375 3.375 0 0 0 2.456 2.456L21.75 6l-1.035.259a3.375 3.375 0 0 0-2.456 2.456ZM16.894 20.567 16.5 21.75l-.394-1.183a2.25 2.25 0 0 0-1.423-1.423L13.5 18.75l1.183-.394a2.25 2.25 0 0 0 1.423-1.423l.394-1.183.394 1.183a2.25 2.25 0 0 0 1.423 1.423l1.183.394-1.183.394a2.25 2.25 0 0 0-1.423 1.423Z"
                    />
                  </svg>
                </button>

                {isOpen && (
                  <div className="fixed inset-0 flex items-center justify-center bg-black/50 z-50">
                    <div className="bg-[--secondary-color] rounded-2xl p-6 w-[400px] relative text-[--primary-color]">
                      {/* Close button */}
                      <button
                        disabled={isCreatingPost}
                        onClick={() => setIsOpen(false)}
                        className="absolute top-2 right-2 p-2 bg-red-500 hover:text-red-700"
                      >
                        ✖
                      </button>

                      <h2 className="text-lg font-bold mb-4">Create a post with AI</h2>

                      {/* Input field */}
                      <textarea
                        placeholder="Describe the post to create..."
                        value={prompt}
                        onChange={(e) => setPrompt(e.target.value)}
                        className="w-full p-2 m-0  border rounded mb-4 text-[--primary-color] "
                      />

                      {/* Action Button */}
                      <button
                        onClick={() => generatePost(prompt)}
                        className="mt-4 w-full bg-[--primary-color] text-white py-2 rounded-lg hover:bg-[--secondary-color] border border-[--primary-color]"
                      >
                        {isCreatingPost ? "Generating..." : "Generate Post"}
                      </button>
                    </div>
                  </div>
                )}
              </div>
              <button className="btn btn-primary absolute bottom-0 right-0" onClick={OnPostClick} disabled={posting}>
                {posting ? "Posting..." : "Post"}
              </button>
            </div>
          </div>
        )}
        {posts.length === 0 ? (
          <h2 className="text-center text-gray-300 mt-5">No posts avaliable{value === "2" ? " for Following" : ""}</h2>
        ) : (
          <div>
            {posts.map((post) => (
              <Post key={post.id} post={post} />
            ))}
            {isThereMorePosts && value === "1" ? (
              <button
                className="bg-[--primary-color] w-[200px] p-4 ml-[35%] my-5 text-xl text-white rounded-full hover:opacity-50"
                onClick={GetMorePosts}
                disabled={isGetMorePosts}
              >
                {isGetMorePosts ? "Loading more posts..." : "Get More Posts"}
              </button>
            ) : (
              <h2 className="text-center text-gray-300 my-5">No more posts to show</h2>
            )}
          </div>
        )}
      </div>
    );
};

export default Posts;
