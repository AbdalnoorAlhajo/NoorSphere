import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { serverUrl } from "../../utils/global";

const EditAccount = () => {
  const navigate = useNavigate();
  const { token, decoded } = useToken();
  const [formData, setFormData] = useState({
    status: "",
    company: "",
    website: "",
    location: "",
    country: "",
    skills: "",
    bio: "",
  });

  useEffect(() => {
    if (!token) {
      navigate("/login");
      return;
    }

    try {
      if (!decoded.id) console.error("Decoded token does not have 'Id' property.");
    } catch (error) {
      console.error("Failed to decode token:", error);
      navigate("/login");
    }
  }, []);

  const handleSubmit = async (e) => {
    e.preventDefault();
    if (formData.skills === "" || formData.status === "") {
      alert("Skills and Professional Status are required fileds");
      return;
    }

    try {
      const token = localStorage.getItem("token");

      var response = await fetch(`${serverUrl}profiles`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
        body: JSON.stringify(formData),
      });

      if (response.ok) {
        navigate("/home");
        alert("profile created");
      } else {
        console.error("Response json:", await response.json);
      }
    } catch (error) {
      console.error("Error:", error);
      alert("Something went wrong. Please try again.");
    }
  };

  const onChange = (e) => {
    setFormData({ ...formData, [e.target.name]: e.target.value });
  };
  return (
    <>
      <div style={{ marginLeft: "20%", marginRight: "10%", textAlign: "center", justifyItems: "center" }}>
        <form className="main" onSubmit={handleSubmit}>
          <h1 className="form-title">Edit Profile</h1>
          <select onChange={(e) => setFormData({ ...formData, status: e.target.value })}>
            <option value="">*Select Professional Status</option>
            <option value="Junior Developer">Junior Developer</option>
            <option value="Senior Developer">Senior Developer</option>
            <option value="Software Engineering Student">Software Engineering Student</option>
            <option value="Other">Other</option>
          </select>
          <input className="input-style" name="avatar" onChange={onChange} type="file" accept="image/*" />
          <input className="input-style" name="company" onChange={onChange} type="text" placeholder="Company" />
          <input className="input-style" name="website" onChange={onChange} type="text" placeholder="Website" />
          <input className="input-style" name="location" onChange={onChange} type="text" placeholder="Location" />
          <input className="input-style" name="country" onChange={onChange} type="text" placeholder="Country" />
          <input className="input-style" name="skills" onChange={onChange} type="text" placeholder="*Skills" />
          <textarea
            className="input-style"
            name="bio"
            onChange={onChange}
            type="text"
            placeholder="A short bio about yourself"
            style={{ width: "70%", marginLeft: "18%", marginRight: "16%" }}
          />
          <button type="submit" className="btn  btn-primary">
            Submit
          </button>
        </form>
      </div>
    </>
  );
};
export default EditAccount;
