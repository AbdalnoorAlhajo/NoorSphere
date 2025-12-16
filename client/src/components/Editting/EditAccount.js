import { useEffect, useState, useRef } from "react";
import { useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { serverUrl } from "../../utils/global";
import axios from "axios";
import { UploadImageToCloudinary } from "../../utils/APIs/imageService";
import toast from "react-hot-toast";

const EditAccount = () => {
  const navigate = useNavigate();
  const { token, decoded } = useToken();
  const HaveProfile = useRef(false);
  const redirectDelayMs = 2000;
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
      toast.error("You can not edit Guest account. You will be redirected to home page.");
      setTimeout(() => navigate("/home"), redirectDelayMs);
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
        if (error.response) {
          if (error.response.status === 404) HaveProfile.current = false;
          else {
            toast.error("Failed to fetch profile. Please try again.");
            setTimeout(() => navigate("/login"), redirectDelayMs);
          }
        } else {
          toast.error("An unexpected error occurred. Please try again later.");
          setTimeout(() => navigate("/home"), redirectDelayMs);
        }
      });
  }, [navigate, token, decoded.name]);

  const handleSubmit = async (e) => {
    e.preventDefault();

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
          toast.success("Profile updated successfully!");
          setTimeout(() => navigate("/home"), redirectDelayMs);
        })
        .catch((error) => {
          console.error("Error:", error);
          toast.error("Something went wrong. Please try again.");
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
          toast.success("Profile created successfully!");
          navigate("/home");
        })
        .catch((error) => {
          console.error("Error:", error);
          toast.error("Something went wrong. Please try again.");
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
          toast.success("Image uploaded successfully!");
        } catch (error) {
          toast.error("Image upload failed. Please try again.");
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
          <select required className="input-style" onChange={(e) => setFormData({ ...formData, status: e.target.value })}>
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
