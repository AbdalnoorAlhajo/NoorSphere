import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import proPicture from "../Images/blank-profile-picture.png";
import axios from "axios";
import { useToken } from "./TokenContext";
import { serverUrl } from "../utils/global";

const Developers = () => {
  const navigate = useNavigate();
  const [usersProfile, setUsersProfile] = useState(null);
  const { token } = useToken();

  useEffect(() => {
    axios
      .get(`${serverUrl}profiles`, {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
      })
      .then((response) => {
        setUsersProfile(response.data);
      })
      .catch((error) => {
        if (error.response?.status === 401) navigate("/home");
        else console.log(error);
      });
  }, []);

  return (
    <div style={{ display: "flex", marginLeft: "14%", flexWrap: "wrap" }}>
      {usersProfile ? (
        usersProfile.map((e) => (
          <Link key={e.profileId} to={`${e.profileId}`} state={{ name: e.name }}>
            <div style={{ marginTop: 60, width: 300, textAlign: "center" }}>
              <img className="profile-picture" alt="Profile" title="Profile" src={proPicture} style={{ width: 200 }} />
              <h2>{e.name}</h2>
            </div>
          </Link>
        ))
      ) : (
        <p>Loading profile...</p>
      )}
    </div>
  );
};

export default Developers;
