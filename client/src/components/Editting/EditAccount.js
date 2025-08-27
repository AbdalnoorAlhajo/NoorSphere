import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { serverUrl } from "../../utils/global";
import axios from "axios";
import { UploadImageToCloudinary } from "../../utils/APIs/imageService";

const EditAccount = () => {
  const navigate = useNavigate();
  const { token, decoded } = useToken();
  const HaveProfile = useRef(false);
  const [formData, setFormData] = useState({
    status: "",
    company: "",
    website: "",
    avatarUrl: "",
    location: "",
    country: "",
    skills: "",
    bio: "",
  });

  useEffect(() => {
    if (decoded.name === "Guest") {
      alert("you can not edit Guest account");
      navigate("/home");
      return;
    }

    axios
      .get(`${serverUrl}profiles/me`, {
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
      })
      .then(() => {
        HaveProfile.current = true;
      })
      .catch((error) => {
        console.log(error);
        if (error.response) {
          if (error.response.status === 404) HaveProfile.current = false;
          else {
            alert("Failed to fetch profile. Please try again.");
            navigate("/login");
          }
        } else {
          alert("An unexpected error occurred. Please try again later.");
          navigate("/login");
        }
      });
  }, [navigate, token, decoded.name]);

  const handleSubmit = async (e) => {
    console.log("Form Data:", formData);

    e.preventDefault();
    if (formData.skills === "" || formData.status === "") {
      alert("Skills and Professional Status are required fileds");
      return;
    }

    console.log("Form Data:", formData);
    if (HaveProfile.current)
      axios
        .put(`${serverUrl}profiles`, formData, {
          headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          },
        })
        .then(() => {
          alert("profile Updated");
          navigate("/home");
        })
        .catch((error) => {
          console.error("Error:", error);
          alert("Something went wrong. Please try again.");
        });
    else
      axios
        .post(`${serverUrl}profiles`, formData, {
          headers: {
            "Content-Type": "application/json",
            Authorization: "Bearer " + token,
          },
        })
        .then(() => {
          alert("profile created");
          navigate("/home");
        })
        .catch((error) => {
          console.error("Error:", error);
          alert("Something went wrong. Please try again.");
        });
  };

  const onChange = async (e) => {
    if (e.target.name !== "avatar") {
      setFormData({ ...formData, [e.target.name]: e.target.value });
    } else {
      const file = e.target.files[0];

      if (!file) return;

      const UploadImage = async () => {
        try {
          const imageURL = await UploadImageToCloudinary(file);
          setFormData((prev) => ({ ...prev, avatarUrl: imageURL }));
        } catch (error) {
          alert("Image upload failed. Please try again.");
        }
      };
      await UploadImage();
    }
  };

  return (
    <>
      <div className="mx-[5%] lg:mx-[10%] text-center justify-items-center">
        <form className="main" onSubmit={handleSubmit}>
          <h1 className="form-title">{HaveProfile.current ? "Edit" : "Create"} Profile</h1>
          <select className="input-style" onChange={(e) => setFormData({ ...formData, status: e.target.value })}>
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
          <input className="input-style" name="skills" onChange={onChange} type="text" placeholder="*Skills" required />
          <textarea className="input-style" name="bio" onChange={onChange} type="text" placeholder="A short bio about yourself" required />
          <button type="submit" className="btn  btn-primary">
            Submit
          </button>
        </form>
      </div>
    </>
  );
};
export default EditAccount;
