import { useEffect } from "react";
import { useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { deleteUserAccount } from "../../utils/APIs/userService";
import Confirm from "../Alerts/Confirm";
import { useState } from "react";
import toast, { Toaster } from "react-hot-toast";

const Settings = () => {
  const navigate = useNavigate();
  const { token, decoded } = useToken();
  const [showConfirm, setShowConfirm] = useState(false);
  const [isDeleteButtonClicked, setIsDeleteButtonClicked] = useState(false);

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

    return () => toast.remove();
  }, [navigate, token, decoded]);

  const handleDeleteClick = async () => {
    if (decoded.name === "Guest") {
      toast.error("you can not delete Guest account");
      setIsDeleteButtonClicked(false);
      return;
    }
    setShowConfirm(true);
  };

  const handleOnEditClick = () => {
    if (decoded.name === "Guest") toast.error("you can not edit Guest account");
    else navigate("/profile/edit");
  };

  const confirmDelete = async () => {
    setIsDeleteButtonClicked(true);
    try {
      await deleteUserAccount(localStorage.getItem("token"));
      localStorage.removeItem("token");
      navigate("/login");
    } catch (error) {
      toast.error("Failed to delete account: " + error.message);
    } finally {
      setIsDeleteButtonClicked(false);
    }
  };
  const BoxStyle = { boxShadow: "0px 11px 35px 2px rgba(0, 0, 0, 0.14)", padding: 20, width: "70%", height: 150, marginBottom: 50 };

  return (
    <div className="pt-[100px] text-center justify-items-center">
      <Toaster position="top-center" />
      <div style={BoxStyle}>
        <p className="text-white my-5">Update your profile Information</p>
        <button className="btn btn-primary" style={{ width: 200 }} onClick={handleOnEditClick}>
          Edit Account
        </button>
      </div>
      {showConfirm && <Confirm isOpen={showConfirm} setIsOpen={setShowConfirm} confirmDelete={confirmDelete} />}
      <div style={BoxStyle}>
        <p className="text-white my-3">Delete your profile</p>
        <button className="btn btn-danger" onClick={handleDeleteClick} disabled={isDeleteButtonClicked}>
          {isDeleteButtonClicked ? "Deleting account..." : "Delete Account"}
        </button>
      </div>
    </div>
  );
};

export default Settings;
