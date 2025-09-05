import { Link } from "react-router-dom";
import LandingTitle from "./LandingTitle";
import { loginUser } from "../../utils/APIs/userService";
import { useEffect } from "react";

const Landing = () => {
  useEffect(() => {
    const warmBackend = async () => {
      try {
        const guestCredentials = { email: "Guest@noorsphere.com", password: "ilovenoorsphere" };
        await loginUser(guestCredentials);
      } catch (error) {
        console.error("Backend warm-up failed:", error);
      }
    };
    warmBackend();
  }, []);

  return (
    <div className="landing">
      <div className="dark-overlay">
        <div className="landing-inner">
          <h1 className="text-3xl">NoorSphere</h1>
          <LandingTitle />
          <div>
            <Link to="/register" className="btn btn-primary block">
              Sign Up
            </Link>
            <Link to="/login" className="btn btn-light block">
              Login
            </Link>
          </div>
        </div>
      </div>
    </div>
  );
};

export default Landing;
