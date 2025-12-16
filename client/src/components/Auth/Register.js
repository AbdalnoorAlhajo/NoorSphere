import { useState, useEffect } from "react";
import { Link, useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";
import { registerUser } from "../../utils/APIs/userService";
import toast from "react-hot-toast";

const Register = () => {
  const [formInput, setFormInput] = useState({ name: "", email: "", password: "" });
  const [confirmPassword, setConfirmPassword] = useState("");
  const navigate = useNavigate();
  const { saveToken } = useToken();
  const [registering, setRegistering] = useState(false);

  useEffect(() => {
    return () => toast.remove();
  }, []);

  async function handleSubmit(e) {
    e.preventDefault();
    setRegistering(true);

    if (formInput.password !== confirmPassword) {
      toast.error("Passwords do not match");
      setRegistering(false);
      return;
    }

    const register = async () => {
      try {
        const response = await registerUser(formInput);

        saveToken(response.data.token);
        toast.success("Registration successful!");
        navigate("/home");
      } catch (error) {
        toast.error(error.message);
      } finally {
        setRegistering(false);
      }
    };
    register();
  }

  return (
    <div className="flex justify-center items-center h-screen text-center  sm:mt-[20%] lg:mt-[5%]">
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

        <button type="submit" className="btn btn-primary btn-comment" disabled={registering}>
          {registering ? "Registering..." : "Register"}
        </button>
        <p>
          <Link to="/login">Already have account in NoorSphere? Login</Link>
        </p>
      </form>
    </div>
  );
};

export default Register;
