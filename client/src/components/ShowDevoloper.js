import React, { useEffect, useState } from "react";
import { useNavigate, useLocation, useParams } from "react-router-dom";
import proPicture from "../Images/blank-profile-picture.png";
import Education from "./Profile/Education";
import Experience from "./Profile/Experience";
import axios from "axios";
import { useToken } from "./TokenContext";
import { serverUrl } from "../utils/global";

const ShowDeveloper = () => {
  const navigate = useNavigate();

  const [DeveloperProfile, setDeveloperProfile] = useState(null);
  const { name } = useLocation().state;
  const { id } = useParams();
  const { token } = useToken();

  useEffect(() => {
    axios
      .get(`${serverUrl}profiles/${id}`, {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
      })
      .then((response) => {
        setDeveloperProfile(response.data);
      })
      .catch((error) => {
        if (error.response?.status === 401) navigate("/home");
        else console.log(error);
      });
  }, []);

  const ProfileInfo = { marginLeft: "15%", marginTop: "1%", width: "75%" };
  if (name == null || DeveloperProfile == null) return <h1>data not exist</h1>;
  return (
    <>
      {DeveloperProfile ? (
        <div className="home">
          <div className="home-row">
            <div className="home-column">
              <img className="profile-picture" alt="profile" src={proPicture} />
              <h2 className="name" style={{ textAlign: "center" }}>
                {name}
              </h2>
            </div>
            <div className="home-column" style={{ marginTop: 80, marginLeft: 60, width: 400, height: 300, textAlign: "center" }}>
              <>
                <div className="container">
                  <p>
                    <b>{DeveloperProfile.bio || "No description provided"}</b>
                  </p>
                </div>
                <div className="container">
                  <p>
                    I live in <b>{DeveloperProfile.location}</b>
                  </p>
                </div>
                <div className="container">
                  <p>
                    From <b>{DeveloperProfile.country}</b>
                  </p>
                </div>
                <div className="container">
                  <b>Skills: </b>
                  {DeveloperProfile.skills}
                </div>
              </>
            </div>
          </div>
          <div className="home-row">
            <div className="home-column">
              <div className="home-row" style={{ marginLeft: "15%" }}>
                <div className="home-column">
                  <h3>Education</h3>
                </div>
              </div>
              <div style={ProfileInfo}>{DeveloperProfile ? <Education education={DeveloperProfile.educations} /> : <p>Loading education...</p>}</div>
            </div>
            <div className="home-column">
              <div className="home-row" style={{ marginLeft: "15%" }}>
                <div className="home-column">
                  <h3>Experience</h3>
                </div>
              </div>
              <div style={ProfileInfo}>
                {DeveloperProfile ? <Experience experience={DeveloperProfile.experiences} /> : <p>Loading experience...</p>}
              </div>
            </div>
          </div>
        </div>
      ) : (
        <p>Loading profile...</p>
      )}
    </>
  );
};

export default ShowDeveloper;
