import { useEffect, useState } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import { useToken } from "../TokenContext";
import { Avatar } from "@mui/material";
import { formatDate } from "../../utils/global";
import { Tabs, Tab } from "@mui/material";
import Post from "../Posts/Post";
import { getMyProfile, getProfileById, GetFollowersAndFollowing } from "../../utils/APIs/profileService";
import { GetPostsByUserId, GetPostsLikedByUser } from "../../utils/APIs/postService";

const ShowProfile = ({ isAnotherProfile }) => {
  const [userProfile, setUserProfile] = useState({});
  const { token, decoded } = useToken();
  const navigate = useNavigate();
  const [value, setValue] = useState("1");
  const [posts, setPosts] = useState([]);
  const [FollowersAndFollwoing, setFollowersAndFollwoing] = useState([]);
  const [isLoading, setIsLoading] = useState(true);
  const { id } = useParams();

  useEffect(() => {
    if (!token) {
      navigate("/login");
      return;
    }
    setIsLoading(true);

    const fetchProfile = async () => {
      if (isAnotherProfile) {
        // Fetch another user's profile
        return await getProfileById(id, token)
          .then((data) => data)
          .catch((error) => {
            if (error.response?.status === 401) navigate("/home");
            else console.log(error);
          });
      } else {
        // Fetch my profile
        return await getMyProfile(token)
          .then((data) => data)
          .catch((error) => {
            if (error.response?.status === 404) {
              alert("You do not have a profile, please create one.");
              navigate("/profile/edit");
            } else {
              alert("Failed to fetch profile. Please try again.");
              navigate("/login");
            }
          });
      }
    };

    const fetchUserPosts = async (userId) => {
      try {
        return await GetPostsByUserId(token, userId);
      } catch (error) {
        console.error("Error fetching user posts:", error);
        return [];
      }
    };

    const fetchPostsLikedByUser = async (userId) => {
      try {
        return await GetPostsLikedByUser(token, userId);
      } catch (error) {
        console.error("Error fetching user posts:", error);
        return [];
      }
    };

    const fetchFollowersAndFollowing = async (userId) => {
      try {
        return await GetFollowersAndFollowing(token, userId);
      } catch (error) {
        console.error("Error fetching user posts:", error);
        return [];
      }
    };

    const fetchAll = async () => {
      const profile = await fetchProfile();
      setUserProfile(profile);

      const FollowsStatus = await fetchFollowersAndFollowing(profile.userId);
      setFollowersAndFollwoing(FollowsStatus);

      if (value === "1") {
        const posts = await fetchUserPosts(profile.userId);
        setPosts(posts);
      } else if (value === "2") {
        const posts = await fetchPostsLikedByUser(profile.userId);
        setPosts(posts);
      }

      setIsLoading(false);
    };

    fetchAll();
  }, [token, decoded, navigate, value, isAnotherProfile, id]);

  const handleChange = (event, newValue) => {
    setValue(newValue);
  };

  if (isLoading) return <div className="text-white m-6">Loading...</div>;

  return (
    <div className="p-3">
      <div className="flex justify-between place-items-center">
        <Avatar
          className=" border-[--primary-color] border-8 border-solid"
          alt={userProfile.name}
          src={userProfile.avatarUrl}
          sx={{ width: 150, height: 150 }}
        />
        {!isAnotherProfile && (
          <Link className="bg-[--primary-color] w-30 p-4 text-center text-white rounded-full hover:opacity-50" to={"/profile/edit"}>
            Edit profile
          </Link>
        )}
      </div>
      <div className="text-blue-300">
        <p className="text-3xl text-[--primary-color] font-bold mt-5 block">{userProfile?.name}</p>
        <div className="my-2 flex items-center">
          <svg
            xmlns="http://www.w3.org/2000/svg"
            fill="none"
            viewBox="0 0 24 24"
            strokeWidth={1.5}
            stroke="currentColor"
            className="size-6 text-[--primary-color]"
          >
            <path
              strokeLinecap="round"
              strokeLinejoin="round"
              d="M6.75 3v2.25M17.25 3v2.25M3 18.75V7.5a2.25 2.25 0 0 1 2.25-2.25h13.5A2.25 2.25 0 0 1 21 7.5v11.25m-18 0A2.25 2.25 0 0 0 5.25 21h13.5A2.25 2.25 0 0 0 21 18.75m-18 0v-7.5A2.25 2.25 0 0 1 5.25 9h13.5A2.25 2.25 0 0 1 21 11.25v7.5m-9-6h.008v.008H12v-.008ZM12 15h.008v.008H12V15Zm0 2.25h.008v.008H12v-.008ZM9.75 15h.008v.008H9.75V15Zm0 2.25h.008v.008H9.75v-.008ZM7.5 15h.008v.008H7.5V15Zm0 2.25h.008v.008H7.5v-.008Zm6.75-4.5h.008v.008h-.008v-.008Zm0 2.25h.008v.008h-.008V15Zm0 2.25h.008v.008h-.008v-.008Zm2.25-4.5h.008v.008H16.5v-.008Zm0 2.25h.008v.008H16.5V15Z"
            />
          </svg>

          <small className="ml-5 text-gray-200 opacity-80">
            Joined at {userProfile.date ? formatDate(userProfile.date) : formatDate(Date.now())}
          </small>
        </div>
        <div>
          <p>
            <b>{userProfile.status}</b>
          </p>
          <p>
            Live in <b>{userProfile.location}</b>
          </p>
          <p>
            <b>{userProfile.bio}</b>
          </p>
          <p>
            <b>Skills:</b> {userProfile.skills}
          </p>
        </div>

        <div className="flex m-3 text-[--primary-color] ">
          <span className="mr-5">{FollowersAndFollwoing.following} Following</span>
          <span> {FollowersAndFollwoing.followers} Followers</span>
        </div>

        <div className="my-5">
          <Tabs value={value} onChange={handleChange} aria-label="Fillter posts">
            <Tab label="Posts" value="1" />
            <Tab label="Likes" value="2" />
          </Tabs>
        </div>
        {posts.length === 0 ? (
          <h2 className="text-center text-gray-300 mt-5">No post found</h2>
        ) : (
          posts.map((post) => <Post key={post.id} post={post} />)
        )}
      </div>
    </div>
  );
};

export default ShowProfile;
