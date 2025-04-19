import React, { useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import axios from "axios";
import { useToken } from "./TokenContext";
import { serverUrl } from "../utils/global";
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
  }, []);

  const handleDeleteClick = async () => {
    const confirmation = window.confirm("Are you sure you want to delete your account? This action cannot be undone.");
    if (!confirmation) return;

    axios
      .delete(`${serverUrl}api/users`, {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + localStorage.getItem("token"),
        },
      })
      .then((response) => {
        alert(response.data);
        localStorage.removeItem("token");
        navigate("/login");
      })
      .catch((error) => {
        alert("Failed to delete account: " + error.response);
      });
  };

  const BoxStyle = { boxShadow: "0px 11px 35px 2px rgba(0, 0, 0, 0.14)", padding: 20, width: "70%", height: 150, marginBottom: 50 };
  const BottomStyle = { color: "white", left: "30%", width: 200 };

  return (
    <div style={{ paddingTop: 100, textAlign: "center", justifyItems: "center" }}>
      <div style={BoxStyle}>
        <p style={{ marginBottom: 20 }}>Update your profile Information</p>
        <Link className="btn" style={{ ...BottomStyle, backgroundColor: "blue" }} to={"/profile/edit"}>
          Edit Account
        </Link>
      </div>

      <div style={BoxStyle}>
        <p>Delete your profile</p>
        <button className="btn" style={{ ...BottomStyle, backgroundColor: "red" }} onClick={handleDeleteClick}>
          Delete Account
        </button>
      </div>
    </div>
  );
};

export default Settings;
