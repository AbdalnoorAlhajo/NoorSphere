import { Link, useNavigate } from "react-router-dom";
import { useState, React } from "react";
import axios from "axios";
import { useToken } from "./TokenContext";
import { serverUrl } from "../utils/global";

const Login = () => {
  const [formInput, setFormInput] = useState({ email: "", password: "" });
  const navigate = useNavigate();
  const { saveToken } = useToken();

  const inputStyle = { display: "block", margin: 10, padding: "14px", width: "100%" };
  const formStyle = { boxShadow: "0px 11px 35px 2px rgba(0, 0, 0, 0.14)", padding: 50 };

  function handleSubmit(e) {
    e.preventDefault();

    if (formInput.password.length < 6) {
      alert("Password must be more than 6 characters");
      return;
    }

    axios
      .post(`${serverUrl}Auth/login`, formInput, {
        headers: {
          "Content-Type": "application/json",
        },
      })
      .then((response) => {
        if (response.data.token) {
          saveToken(response.data.token);
          navigate("/home");
        } else alert(response?.errors?.[0]?.msg || "Something went wrong. Please try again.");
      })
      .catch((error) => {
        if (error.response) {
          console.log("error", error);

          if (error.response.status === 400) alert("Bad request. Please check your input.");
          else if (error.response.status === 404) alert("User not found. Please check your credentials.");
          else if (error.response.status === 500) alert("Internal server error. Please try again later.");
          else alert("Something went wrong. Please try again.");
        } else alert("An unexpected error occurred. Please try again later.");
      });
  }

  return (
    <div style={{ marginTop: "200px", textAlign: "center", justifyItems: "center" }}>
      <form style={formStyle} onSubmit={handleSubmit}>
        <h2 style={{ padding: "20px", fontSize: 30 }}>Sign In</h2>

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

        <button type="submit" className="btn btn-primary">
          Login
        </button>

        <p>
          <Link to="/register">New to NoorSphere? Sign up</Link>
        </p>
      </form>
    </div>
  );
};

export default Login;
