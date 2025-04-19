import axios from "axios";
import { React, useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { serverUrl } from "../utils/global";

const Register = () => {
  const [formInput, setFormInput] = useState({ name: "", email: "", password: "" });
  const [confirmPassword, setConfirmPassword] = useState("");
  const navigate = useNavigate();

  const inputStyle = { display: "block", margin: 10, padding: "14px", width: "100%" };
  const formStyle = { boxShadow: "0px 11px 35px 2px rgba(0, 0, 0, 0.14)", padding: 50 };

  async function handleSubmit(e) {
    e.preventDefault();

    if (formInput.password !== confirmPassword) {
      alert("Passwords do not match");
      return;
    }
    axios
      .post(`${serverUrl}Auth/register`, formInput, {
        headers: {
          "Content-Type": "application/json",
        },
      })
      .then((response) => {
        localStorage.setItem("token", response.data.token);
        navigate("/home");
      })
      .catch((error) => {
        if (error.response) {
          console.log("error", error);

          if (error.response.status === 400) alert("Bad request: User Name or email are already taken");
          else if (error.response.status === 500) alert("Internal server error. Please try again later.");
          else alert("Something went wrong. Please try again.");
        } else alert("An unexpected error occurred. Please try again later.");
      });
  }

  return (
    <div style={{ marginTop: "200px", textAlign: "center", justifyItems: "center" }}>
      <form onSubmit={handleSubmit} style={formStyle}>
        <h2 style={{ padding: "20px", fontSize: 30 }}>Sign Up</h2>

        <input
          type="text"
          required
          placeholder="Name"
          value={formInput.name}
          onChange={(event) => {
            setFormInput({ ...formInput, name: event.target.value });
          }}
          style={inputStyle}
        />

        <input
          type="email"
          required
          placeholder="Write Email"
          value={formInput.email}
          onChange={(event) => {
            setFormInput({ ...formInput, email: event.target.value });
          }}
          style={inputStyle}
        />

        <input
          type="password"
          required
          placeholder="Password"
          value={formInput.password}
          onChange={(event) => {
            setFormInput({ ...formInput, password: event.target.value });
          }}
          style={inputStyle}
        />
        <input
          required
          type="password"
          placeholder="Confirm Password"
          value={confirmPassword}
          onChange={(event) => {
            setConfirmPassword(event.target.value);
          }}
          style={inputStyle}
        />

        <button type="submit" className="btn btn-primary">
          Register
        </button>
        <p>
          <Link to="/login">Already have account in NoorSphere? Login</Link>
        </p>
      </form>
    </div>
  );
};

export default Register;
