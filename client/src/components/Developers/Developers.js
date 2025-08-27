import { useEffect, useState } from "react";
import { useNavigate, useLocation } from "react-router-dom";
import { useToken } from "../TokenContext";
import Search from "../Navbars/LeftNavComps/Search";
import LeftSideNavbar from "../Navbars/LeftSideNavbar";
import NavbarPhone from "../Navbars/NavbarPhone";
import Glowing from "../Navbars/LeftNavComps/Glowing";
import { getProfiles } from "../../utils/APIs/userService";
import SuggestedPerson from "../Navbars/LeftNavComps/SuggestedPerson";
import { GetFollowingSuggestionsByName } from "../../utils/APIs/profileService";
import Following from "../Navbars/LeftNavComps/Following";

const Developers = () => {
  const navigate = useNavigate();
  const [usersProfile, setUsersProfile] = useState(null);
  const { token } = useToken();
  const location = useLocation();
  const query = new URLSearchParams(location.search).get("query");

  useEffect(() => {
    if (!query) {
      const GetProfiles = async () => {
        try {
          const data = await getProfiles(token);
          setUsersProfile(data);
        } catch (error) {
          if (error.response?.status === 401) {
            navigate("/home");
          } else {
            console.error(error);
          }
        }
      };
      GetProfiles();
    } else {
      const SearchForSpecificUser = async () => {
        try {
          const data = await GetFollowingSuggestionsByName(token, query);
          setUsersProfile(data);
        } catch (error) {
          if (error.response?.status === 401) {
            navigate("/home");
          } else {
            console.error(error);
            setUsersProfile([]);
          }
        }
      };
      SearchForSpecificUser();
    }
  }, [token, navigate, query]);

  return (
    <div className=" overflow-auto bg-[--secondary-color]">
      {/* Phone Navbar */}
      <NavbarPhone className="navs-Phone" />
      <div className="grid grid-cols-6 gap-4 xl:mx-20 relative h-screen">
        {/* Left Side Navbar */}
        <LeftSideNavbar />

        {/* Main Content Area */}
        <div className="col-span-6 xl:col-span-3  sm:px-10 lg:px-20 xl:px-0">
          <Search type={"developers"} />
          <div className="m-6">
            <h2 className="text-xl font-semibold text-[--primary-color]">Expand your network</h2>
            <p className="text-white my-6">Use the search bar above to find people.</p>
          </div>
          {query ? (
            <div className="mx-6">
              <h2 className="text-xl font-semibold text-[--primary-color]">Search Results for: {query}</h2>
              {!usersProfile ? (
                <p className="text-white my-6">Loading...</p>
              ) : usersProfile.length === 0 ? (
                <p className="text-white my-6">No results found.</p>
              ) : (
                <div>
                  {usersProfile.map((profile) => (
                    <div key={profile.id} className="flex items-center m-5 p-3">
                      <SuggestedPerson user={profile} />
                    </div>
                  ))}
                </div>
              )}
            </div>
          ) : (
            <div className="lg:mx-[150px]">
              <Following />
            </div>
          )}
        </div>

        {/* Right Side Navbar */}
        <div className="col-span-2 hidden xl:block">
          {/* What is Glowing */}
          <Glowing />
        </div>
      </div>
    </div>
  );
};

export default Developers;

//test
// import { useState } from "react";

// export default function Developers() {
//   const [isOpen, setIsOpen] = useState(false);
//   const [prompt, setPrompt] = useState("");

//   return (
//     <div className="p-6">
//       {/* Trigger Button */}
//       <button onClick={() => setIsOpen(true)} className="p-2 rounded-full bg-blue-500 text-white hover:bg-blue-600">
//         ✨
//       </button>

//       {/* Modal */}
//       {isOpen && (
//         <div className="fixed inset-0 flex items-center justify-center bg-black/50 z-50">
//           <div className="bg-white rounded-2xl p-6 w-[400px] relative">
//             {/* Close button */}
//             <button onClick={() => setIsOpen(false)} className="absolute top-2 right-2 text-gray-500 hover:text-black">
//               ✖
//             </button>

//             <h2 className="text-lg font-bold mb-4">Create an image with AI</h2>

//             {/* Input field */}
//             <textarea
//               placeholder="Describe the image to create..."
//               value={prompt}
//               onChange={(e) => setPrompt(e.target.value)}
//               className="w-full p-2 m-0  border rounded mb-4"
//             />

//             {/* Preview cards */}
//             <div className="grid grid-cols-2 gap-2">
//               <div className="h-28 bg-yellow-200 rounded flex items-center justify-center">Example 1</div>
//               <div className="h-28 bg-blue-200 rounded flex items-center justify-center">Example 2</div>
//             </div>

//             {/* Action Button */}
//             <button
//               onClick={() => alert(`Generating image for: ${prompt}`)}
//               className="mt-4 w-full bg-blue-500 text-white py-2 rounded-lg hover:bg-blue-600"
//             >
//               Generate Image
//             </button>
//           </div>
//         </div>
//       )}
//     </div>
//   );
// }
