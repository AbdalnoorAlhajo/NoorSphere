import { Link, Outlet, useNavigate } from "react-router-dom";
import { loginUser } from "../../utils/APIs/userService";
import { useToken } from "../TokenContext";
import { useState } from "react";
import toast from "react-hot-toast";

const Navbar = () => {
  const navigate = useNavigate();
  const { saveToken } = useToken();
  const [logging, setLogging] = useState(false);

  const LoginAsGuest = async () => {
    setLogging(true);
    try {
      const guestCredentials = { email: "Guest@noorsphere.com", password: "ilovenoorsphere" };
      const response = await loginUser(guestCredentials);

      saveToken(response.data.token);
      toast.success("Logged in as Guest!");
      navigate("/home");
    } catch (error) {
      toast.error(error.message);
    } finally {
      setLogging(false);
    }
  };
  return (
    <>
      <nav className="navbar bg-navbar justify-between">
        <h1>
          <Link className="sm:text-lg lg:text-2xl" to="/">
            NoorSphere
          </Link>
        </h1>
        <ul className="flex">
          <li>
            <button onClick={LoginAsGuest} className="btn btn-primary btn-guest" disabled={logging}>
              {logging ? "Logging in..." : "Login as Guest"}
            </button>
          </li>
        </ul>
      </nav>
      <Outlet />
    </>
  );
};

export default Navbar;
