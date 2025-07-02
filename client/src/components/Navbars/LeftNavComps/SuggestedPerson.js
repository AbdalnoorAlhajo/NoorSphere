import { Avatar } from "@mui/material";
import { addFollow } from "../../../utils/APIs/userService";
import { useToken } from "../../TokenContext";
import { useNavigate } from "react-router-dom";
import { getMyProfile } from "../../../utils/APIs/profileService";
import defaultAvatar from "../../../Images/blank-profile-picture.png";

const SuggestedPerson = ({ user }) => {
  const { token, decoded } = useToken();
  const navigate = useNavigate();

  const handleOnFollowClick = async (FollowedUserId) => {
    const fetchProfile = async () => {
      return await getMyProfile(token)
        .then((data) => data)
        .catch((error) => {
          if (error.response?.status === 404) {
            alert("You do not have a profile, please create one before following.");
            navigate("/profile/edit");
          } else {
            alert("Failed to fetch profile. Please try again.");
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
        alert("User followed successfully!");
      })
      .catch((error) => {
        console.error("Error adding follow:", error.response);
      });
  };

  return (
    <>
      <Avatar src={user.avatarUrl ?? defaultAvatar} alt={user.name}>
        {user.name?.[0] ?? "?"}
      </Avatar>
      <p className="text-lg mx-5 w-40 text-white">{user.name}</p>
      <button onClick={() => handleOnFollowClick(user.userId)} className="btn btn-primary">
        Follow
      </button>
    </>
  );
};

export default SuggestedPerson;
