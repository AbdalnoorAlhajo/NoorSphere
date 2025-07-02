import { Link } from "react-router-dom";
import { Outlet } from "react-router-dom";
import { loginUser } from "../../utils/APIs/userService";
import { useNavigate } from "react-router-dom";
import { useToken } from "../TokenContext";

const Navbar = () => {
  const navigate = useNavigate();
  const { saveToken } = useToken();

  const LoginAsGuest = async () => {
    try {
      const guestCredentials = { email: "Guest@noorsphere.com", password: "ilovenoorsphere" };
      const response = await loginUser(guestCredentials);

      saveToken(response.data.token);
      navigate("/home");
    } catch (error) {
      alert(error.message);
    }
  };
  return (
    <>
      <nav className="navbar bg-navbar justify-between">
        <h1>
          <Link className="text-2xl" to="/">
            NoorSphere
          </Link>
        </h1>
        <ul className="flex">
          <li>
            <button onClick={LoginAsGuest} className="btn btn-primary btn-guest">
              Login as Guest
            </button>
          </li>
        </ul>
      </nav>
      <Outlet />
    </>
  );
};

export default Navbar;
