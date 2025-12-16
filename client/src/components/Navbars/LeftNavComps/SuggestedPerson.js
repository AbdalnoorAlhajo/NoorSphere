import { Avatar } from "@mui/material";
import { addFollow } from "../../../utils/APIs/userService";
import { useToken } from "../../TokenContext";
import { Link, useNavigate } from "react-router-dom";
import { getMyProfile } from "../../../utils/APIs/profileService";
import defaultAvatar from "../../../Images/blank-profile-picture.png";
import toast from "react-hot-toast";

const SuggestedPerson = ({ user }) => {
  const { token, decoded } = useToken();
  const navigate = useNavigate();

  const handleOnFollowClick = async (FollowedUserId) => {
    const fetchProfile = async () => {
      return await getMyProfile(token)
        .then((data) => data)
        .catch((error) => {
          if (error.response?.status === 404) {
            toast.error("You do not have a profile, please create one before following.");
            navigate("/profile/edit");
          } else {
            toast.error("Failed to fetch profile. Please try again.");
            navigate("/login");
          }
        });
    };
    await fetchProfile();

    const addNewFollowDTO = {
      FollowerUserId: decoded.id, // current user
      FollowedUserId: FollowedUserId, // user to be followed
    };

    await addFollow(token, addNewFollowDTO)
      .then(() => {
        toast.success("User followed successfully!");
      })
      .catch((error) => {
        console.error("Error adding follow:", error.response);
        toast.error("Failed to follow user. Please try again.");
      });
  };

  return (
    <div className="flex">
      <Link className="flex items-center my-3 cursor-pointer" to={`/showDeveloper/${user.userId}`}>
        <Avatar src={user.avatarUrl ?? defaultAvatar} alt={user.name}>
          {user.name?.[0] ?? "?"}
        </Avatar>
        <p className="text-lg mx-2 w-28 text-white">{user.name}</p>
      </Link>

      <button onClick={() => handleOnFollowClick(user.userId)} className="btn btn-primary">
        Follow
      </button>
    </div>
  );
};

export default SuggestedPerson;
