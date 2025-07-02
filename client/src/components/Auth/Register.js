import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { registerUser } from "../../utils/APIs/userService";

const Register = () => {
  const [formInput, setFormInput] = useState({ name: "", email: "", password: "" });
  const [confirmPassword, setConfirmPassword] = useState("");
  const navigate = useNavigate();
  const { saveToken } = useToken();

  async function handleSubmit(e) {
    e.preventDefault();

    if (formInput.password !== confirmPassword) {
      alert("Passwords do not match");
      return;
    }

    const register = async () => {
      try {
        const response = await registerUser(formInput);

        saveToken(response.data.token);
        navigate("/home");
      } catch (error) {
        alert(error.message);
      }
    };
    register();
  }

  return (
    <div className="flex justify-center items-center h-screen text-center">
      <form onSubmit={handleSubmit} className="shadow-xl p-12">
        <h2 className="p-[20px] text-[30px]">Sign Up</h2>

        <input
          type="text"
          required
          placeholder="Name"
          value={formInput.name}
          onChange={(event) => {
            setFormInput({ ...formInput, name: event.target.value });
          }}
          className="input-style"
        />

        <input
          type="email"
          required
          placeholder="Write Email"
          value={formInput.email}
          onChange={(event) => {
            setFormInput({ ...formInput, email: event.target.value });
          }}
          className="input-style"
        />

        <input
          type="password"
          required
          placeholder="Password"
          value={formInput.password}
          onChange={(event) => {
            setFormInput({ ...formInput, password: event.target.value });
          }}
          className="input-style"
        />
        <input
          required
          type="password"
          placeholder="Confirm Password"
          value={confirmPassword}
          onChange={(event) => {
            setConfirmPassword(event.target.value);
          }}
          className="input-style"
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
