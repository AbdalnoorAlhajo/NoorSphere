import { useState, useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { Tab, Tabs } from "@mui/material";
import Post from "../Posts/Post";
import { GetAllPosts, SavePost, GetPostsByFollowedUsers } from "../../utils/APIs/postService";
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

  const navigate = useNavigate();

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  useEffect(() => {
    setIsLoading(true);

    if (value === "1") {
      const fetchPosts = async () => {
        try {
          const data = await GetAllPosts(token);
          setPosts(data);
        } catch (error) {
          alert(error.message);
          if (error.message === "Unauthorized") {
            navigate("/login");
          }
        } finally {
          setIsLoading(false);
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
  }, [token, navigate, value]);

  const OnPostClick = async () => {
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

    if (!profile) return;

    setPosting(true);

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

  if (isLoading) return <div>Loading...</div>;
  else
    return (
      <div className="container">
        <div>
          <Tabs value={value} onChange={handleChange} aria-label="Fillter posts">
            <Tab label="All" value="1" />
            <Tab label="Following" value="2" />
          </Tabs>
        </div>
        {value === "2" ? (
          ""
        ) : (
          <div className="post">
            <input
              type="text"
              className="bg-inherit pb-6 text-xl outline-none  placeholder:text-[--primary-color] text-[--primary-color]"
              value={inputValue}
              onChange={(e) => setInputValue(e.target.value)}
              placeholder="What is in your mind?"
            />

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
          posts.map((post) => <Post key={post.id} post={post} />)
        )}
      </div>
    );
};

export default Posts;
