import { useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { deleteUserAccount } from "../../utils/APIs/userService";

const Settings = () => {
  const navigate = useNavigate();
  const { token, decoded } = useToken();

  useEffect(() => {
    if (!token) {
      navigate("/login");
      return;
    }

    try {
      if (!decoded) console.error("Decoded token does not have 'name' property.");
    } catch (error) {
      console.error("Failed to decode token:", error);
      navigate("/login");
    }
  }, [navigate, token, decoded]);

  const handleDeleteClick = async () => {
    if (decoded.name === "Guest") {
      alert("you can not delete Guest account");
      return;
    }

    const confirmation = window.confirm("Are you sure you want to delete your account? This action cannot be undone.");
    if (!confirmation) return;

    try {
      const message = await deleteUserAccount(localStorage.getItem("token"));
      alert(message);
      localStorage.removeItem("token");
      navigate("/login");
    } catch (error) {
      alert("Failed to delete account: " + error.message);
    }
  };

  const BoxStyle = { boxShadow: "0px 11px 35px 2px rgba(0, 0, 0, 0.14)", padding: 20, width: "70%", height: 150, marginBottom: 50 };

  return (
    <div className="pt-[100px] text-center justify-items-center">
      <div style={BoxStyle}>
        <p className="text-white my-5">Update your profile Information</p>
        <Link className="btn btn-primary" to={"/profile/edit"}>
          Edit Account
        </Link>
      </div>

      <div style={BoxStyle}>
        <p className="text-white my-3">Delete your profile</p>
        <button className="btn btn-danger" onClick={handleDeleteClick}>
          Delete Account
        </button>
      </div>
    </div>
  );
};

export default Settings;
