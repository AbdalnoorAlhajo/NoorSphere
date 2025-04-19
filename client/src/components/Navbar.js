import { Link } from "react-router-dom";
import { Outlet } from "react-router-dom";

const Navbar = () => {
  return (
    <>
      <nav className="navbar bg-navbar justify-between">
        <h1>
          <Link className="text-2xl" to="/">
            NoorSphere
          </Link>
        </h1>
        <ul>
          <li>
            <Link to="/login">Login</Link>
          </li>
        </ul>
      </nav>
      <Outlet />
    </>
  );
};

export default Navbar;
