import { Outlet } from "react-router-dom";
import NavbarPhone from "../Navbars/NavbarPhone";
import Search from "../Navbars/LeftNavComps/Search";
import Glowing from "../Navbars/LeftNavComps/Glowing";
import Following from "../Navbars/LeftNavComps/Following";
import LeftSideNavbar from "../Navbars/LeftSideNavbar";

const Home = () => {
  return (
    <div className=" overflow-auto bg-[--secondary-color]">
      {/* Phone Navbar */}
      <NavbarPhone className="navs-Phone" />

      <div className="grid grid-cols-6 gap-4 xl:mx-20 relative h-screen">
        {/* Left Side Navbar */}
        <LeftSideNavbar />

        {/* Main Content Area */}
        <div className="col-span-6 xl:col-span-3 sm:px-10 lg:px-20 xl:px-0">
          <Outlet />
        </div>

        {/* Right Side Navbar */}
        <div className="col-span-2 hidden xl:block p-5">
          {/* Search Bar */}
          <Search type={"explore"} />

          {/* What is Glowing */}
          <Glowing />

          {/* Who to follow */}
          <Following />
        </div>
      </div>
    </div>
  );
};

export default Home;
