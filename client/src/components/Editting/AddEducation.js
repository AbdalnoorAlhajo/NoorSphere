import { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { serverUrl } from "../../utils/global";

const AddEducation = () => {
  const navigate = useNavigate();
  const { token, decoded } = useToken();

  const [formData, setFormData] = useState({
    school: "",
    degree: "",
    fieldofstudy: "",
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
      if (!decoded.id) console.error("Decoded token does not have 'Id' property.");
    } catch (error) {
      console.error("Failed to decode token:", error);
      navigate("/login");
    }
  }, [navigate, token, decoded]);

  const handleOnSubmit = async (e) => {
    e.preventDefault();
    console.log(formData);
    if (formData.fieldofstudy === "" || formData.school === "" || formData.degree === "") {
      alert("School, Degree, and Field of study are required fileds");
      return;
    }

    try {
      const requestBody = {
        degree: formData.degree,
        fieldofstudy: formData.fieldofstudy,
        from: formData.from,
        to: formData.to,
        school: formData.school,
        current: formData.current,
      };

      const token = localStorage.getItem("token");

      const response = await fetch(`${serverUrl}profiles/education`, {
        method: "POST",
        headers: {
          "Content-Type": "application/json",
          Authorization: "Bearer " + token,
        },
        body: JSON.stringify(requestBody),
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
      <div className="main">
        <h1 className="form-title">Add Education</h1>
        <input className="input-style" name="school" onChange={onChange} type="text" placeholder="*School" />
        <input className="input-style" name="degree" onChange={onChange} type="text" placeholder="*Degree" />
        <input className="input-style" name="fieldofstudy" onChange={onChange} type="text" placeholder="Field of study" />
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
        <input className="input-style" name="to" onChange={onChange} type="date" disabled={formData.current} />
        <button className="btn btn-primary" onClick={handleOnSubmit} style={{ marginLeft: "15%" }}>
          Submit
        </button>
      </div>
    </div>
  );
};
export default AddEducation;
