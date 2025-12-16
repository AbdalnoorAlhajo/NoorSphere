import { useEffect, useState } from "react";
import { useLocation } from "react-router-dom";
import LeftSideNavbar from "../Navbars/LeftSideNavbar";
import NavbarPhone from "../Navbars/NavbarPhone";
import Search from "../Navbars/LeftNavComps/Search";
import Following from "../Navbars/LeftNavComps/Following";
import Glowing from "../Navbars/LeftNavComps/Glowing";
import { useNavigate } from "react-router-dom";
import { GetPostsByText } from "../../utils/APIs/postService";
import { useToken } from "../TokenContext";
import Post from "./Post";
import toast from "react-hot-toast";

const Explore = () => {
  const location = useLocation();
  const query = new URLSearchParams(location.search).get("query");
  const [results, setResults] = useState([]);
  const [loading, setIsLoading] = useState(false);
  const navigate = useNavigate();
  const { token } = useToken();

  useEffect(() => {
    setIsLoading(true);
    if (!query) return;

    const fetchResults = async () => {
      try {
        const data = await GetPostsByText(token, query);
        setResults(data);
      } catch (error) {
        toast.error(error.message);
        setResults([]);
      } finally {
        setIsLoading(false);
      }
    };
    fetchResults();
  }, [token, navigate, query]);

  return (
    <div className=" overflow-auto bg-[--secondary-color] h-screen">
      {/* Phone Navbar */}
      <NavbarPhone className="navs-Phone" />

      <div className="grid grid-cols-6 gap-4 xl:mx-20 relative h-screen">
        {/* Left Side Navbar */}
        <LeftSideNavbar />

        {/* Main Content Area */}
        <div className="col-span-6 xl:col-span-3 lg:px-20 xl:px-0">
          <Search type={"explore"} />
          <div className="mt-6">
            {query ? (
              <div className="mx-2">
                <h2 className="text-xl font-semibold text-[--primary-color]  m-4">Search Results for: {query}</h2>
                {loading ? (
                  <p className="text-white m-6">Loading...</p>
                ) : results.length === 0 ? (
                  <p className="text-white m-6">No results found.</p>
                ) : (
                  <>
                    {results.map((post) => (
                      <Post key={post.id} post={post} />
                    ))}
                  </>
                )}
              </div>
            ) : (
              <div className="mx-6">
                <h2 className="text-xl font-semibold text-[--primary-color] my-6">Start Exploring</h2>
                <p className="text-white my-6">Use the search bar above to find people, topics, or projects</p>
                <Glowing />
              </div>
            )}
          </div>
        </div>

        {/* Right Side Navbar */}
        <div className="col-span-2 hidden xl:block p-5">
          {/* Who to follow */}
          <Following />
        </div>
      </div>
    </div>
  );
};

export default Explore;
