import { Link, Outlet } from "react-router-dom";
import proPicture from "../Images/blank-profile-picture.png";

const SideNavbar = () => {
  const navsStyle = { color: "white", padding: 18, fontSize: 25 };
  return (
    <div className="overflow-auto bg-gray-100 h-screen w-full ">
      <div className="mx-12 relative">
        <nav className="navbar bg-navbar justify-end">
          <ul>
            <li>
              <div className="navs-Phone">
                <Link to="/Home">Home</Link>
                <Link to="/Posts">Posts</Link>
                <Link to="/Developers">Developers</Link>
                <Link to="/Settings">Settings</Link>
              </div>
            </li>
            <li>
              <Link to="/" onClick={() => localStorage.removeItem("token")}>
                Log out
              </Link>
            </li>
          </ul>
        </nav>
        <div className="sidebar">
          <img alt="Profolio" src={proPicture} title="Profolio" className="w-24 m-5 rounded-full" />
          <Link style={navsStyle} to="/home">
            Home
          </Link>
          <Link style={navsStyle} to="/posts">
            Posts
          </Link>
          <Link style={navsStyle} to="/developers">
            Developers
          </Link>
          <Link style={navsStyle} to="/settings">
            Settings
          </Link>
        </div>
        <Outlet />
      </div>
    </div>
  );
};

export default SideNavbar;
