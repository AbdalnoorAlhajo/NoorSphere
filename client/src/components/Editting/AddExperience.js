import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { serverUrl } from "../../utils/global";

const AddExperience = () => {
  const navigate = useNavigate();
  const { token, decoded } = useToken();

  const [formData, setFormData] = useState({
    title: "",
    company: "",
    location: "",
    from: Date,
    to: Date,
    current: false,
  });

  useEffect(() => {
    if (!token) {
      navigate("/login");
      return;
    }

    try {
      if (!decoded) console.error("Decoded token does not have 'Id' property.");
    } catch (error) {
      console.error("Failed to decode token:", error);
      navigate("/login");
    }
  }, [token, decoded, navigate]);

  const handleSubmit = async (e) => {
    e.preventDefault();
    console.log(formData);
    if (formData.title === "" || formData.company === "") {
      alert("Title and Company are required fileds");
      return;
    }

    try {
      const token = localStorage.getItem("token");

      const response = await fetch(`${serverUrl}profiles/experience`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
        body: JSON.stringify(formData),
      });

      const data = await response.json();

      if (response.ok) navigate("/home");
      else alert(data.errors[0].msg);
    } catch (error) {
      console.error("Error:", error);
      alert("Something went wrong. Please try again.");
    }
  };

  const PargraphStyle = { display: "flex", marginLeft: "15%", padding: 10 };

  const onChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };
  return (
    <div style={{ marginLeft: "20%", marginRight: "10%", textAlign: "center", justifyItems: "center" }}>
      <form className="main" onSubmit={handleSubmit}>
        <h1 className="form-title">Add Experience</h1>
        <input className="input-style" name="title" onChange={onChange} type="text" placeholder="*Job title" />
        <input className="input-style" name="company" onChange={onChange} type="text" placeholder="*Company" />
        <input className="input-style" name="location" onChange={onChange} type="text" placeholder="Location" />
        <p style={PargraphStyle}>
          <b>From Date</b>
        </p>
        <input className="input-style" name="from" onChange={onChange} type="date" />
        <p style={PargraphStyle}>
          <input
            name="current"
            onChange={() => {
              setFormData({ ...formData, current: !formData.current });
            }}
            type="checkbox"
          />
          <b>Current School</b>
        </p>
        <p style={PargraphStyle}>
          <b>To Date</b>
        </p>
        <input name="to" onChange={onChange} type="date" className="input-style" disabled={formData.current} />
        <button className="btn  btn-primary" style={{ marginLeft: "15%" }}>
          Submit
        </button>
      </form>
    </div>
  );
};
export default AddExperience;
