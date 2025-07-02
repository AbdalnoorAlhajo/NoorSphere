import { Link, useNavigate } from "react-router-dom";
import { useState } from "react";
import { useToken } from "../TokenContext";
import { loginUser } from "../../utils/APIs/userService";

const Login = () => {
  const [formInput, setFormInput] = useState({ email: "", password: "" });
  const navigate = useNavigate();
  const { saveToken } = useToken();

  function handleSubmit(e) {
    e.preventDefault();

    if (formInput.password.length < 6) {
      alert("Password must be more than 6 characters");
      return;
    }

    const login = async () => {
      try {
        const response = await loginUser(formInput);

        saveToken(response.data.token);
        navigate("/home");
      } catch (error) {
        alert(error.message);
      }
    };
    login();
  }

  return (
    <div className="flex justify-center items-center h-screen text-center">
      <form className="shadow-xl p-12" onSubmit={handleSubmit}>
        <h2 className="p-[20px] text-[30px]">Sign In</h2>

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
