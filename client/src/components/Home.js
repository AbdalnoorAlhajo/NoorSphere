import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import proPicture from "../Images/blank-profile-picture.png";
import { useToken } from "./TokenContext";
import Education from "./Profile/Education";
import Experience from "./Profile/Experience";
import axios from "axios";
import { serverUrl } from "../utils/global";

const Home = () => {
  const [userName, setUserName] = useState("User");
  const [userProfile, setUserProfile] = useState(null);
  const { token, decoded } = useToken();
  const navigate = useNavigate();

  useEffect(() => {
    if (!token) {
      navigate("/login");
      return;
    }

    try {
      if (decoded && decoded.name) setUserName(decoded.name);
      else {
        console.error("Decoded token does not have 'name' property.");
        setUserName("Guest");
      }

      axios
        .get(`${serverUrl}profiles/me`, {
          headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          },
        })
        .then((response) => {
          setUserProfile(response.data);
        })
        .catch((error) => {
          console.log(error);
          if (error.response) {
            if (error.response.status === 404) {
              alert("You do not have a profile, please create one.");
              navigate("/profile/edit");
            } else {
              alert("Failed to fetch profile. Please try again.");
              navigate("/login");
            }
          } else {
            alert("An unexpected error occurred. Please try again later.");
            navigate("/login");
          }
        });
    } catch (error) {
      console.error("Failed to decode token:", error);
      navigate("/login");
    }
  }, []);

  const ButtonStyle = { position: "absolute", right: 0, top: 0 };
  const ProfileInfo = { marginLeft: "15%", marginTop: "1%", width: "75%" };

  if (userProfile == null) return <h1>Loading data</h1>;

  return (
    <div style={{ backgroundColor: "#eaffea" }}>
      <div className="home">
        <div className="home-row">
          <div className="home-column">
            <img className="profile-picture" alt="Profile" title="Profile" src={proPicture} />
            <h2 className="name" style={{ textAlign: "center" }}>
              {userName}
            </h2>
          </div>
          <div className="home-column">
            {userProfile ? (
              <>
                <div className="container">
                  <p>
                    <b>{userProfile.status}</b>
                  </p>
                </div>
                <div className="container">
                  <p>
                    I live in <b>{userProfile.location}</b>
                  </p>
                </div>
                <div className="container">
                  <p>
                    From <b>{userProfile.country}</b>
                  </p>
                </div>
                <div className="container" style={{ textAlign: "initial" }}>
                  <b>Skills: </b>
                  {userProfile.skills}
                </div>
              </>
            ) : (
              <p>Loading profile...</p>
            )}
          </div>
        </div>
        <div className="home-row" style={{ marginTop: 50 }}>
          <div className="home-column" style={{ position: "relative" }}>
            <div className="home-row" style={{ marginLeft: "15%" }}>
              <div className="home-column">
                <h3>Education</h3>
              </div>
              <div className="home-column" style={{ textAlign: "center" }}>
                <Link className="btn btn-primary" style={ButtonStyle} to={"/profile/addEducation"}>
                  Add
                </Link>
              </div>
            </div>
            <div style={ProfileInfo}>{userProfile ? <Education education={userProfile.educations} /> : <p>Loading education...</p>}</div>
          </div>
          <div className="home-column" style={{ position: "relative" }}>
            <div className="home-row" style={{ marginLeft: "15%" }}>
              <div className="home-column">
                <h3>Experience</h3>
              </div>
              <div className="home-column" style={{ textAlign: "center" }}>
                <Link className="btn btn-primary" style={ButtonStyle} to={"/profile/addExperience"}>
                  Add
                </Link>
              </div>
            </div>
            <div style={ProfileInfo}>{userProfile ? <Experience experience={userProfile.experiences} /> : <p>Loading experience...</p>}</div>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Home;
