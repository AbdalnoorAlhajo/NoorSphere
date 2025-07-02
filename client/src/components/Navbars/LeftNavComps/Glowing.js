import { useNavigate } from "react-router-dom";
import { GetTrendingTopics } from "../../../utils/APIs/postService";
import { useState, useEffect } from "react";
import { useToken } from "../../TokenContext";

const Glowing = () => {
  const navigate = useNavigate();
  const [trendingTopics, setTrendingTopics] = useState([]);
  const { token } = useToken();

  const handleClick = (topic) => {
    navigate(`/explore?query=${encodeURIComponent(topic)}`);
  };

  useEffect(() => {
    const fetchTrending = async () => {
      try {
        const trending = await GetTrendingTopics(token);
        setTrendingTopics(trending);
      } catch (error) {
        console.log(error);
      }
    };
    fetchTrending();
  }, [token]);

  return (
    <div className="border-2 border-solid rounded-xl mb-10 border-[--primary-color]">
      <div className="flex items-center p-3">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth={1.5}
          stroke="currentColor"
          className="size-6 mr-6 text-[--primary-color]"
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="m6.115 5.19.319 1.913A6 6 0 0 0 8.11 10.36L9.75 12l-.387.775c-.217.433-.132.956.21 1.298l1.348 1.348c.21.21.329.497.329.795v1.089c0 .426.24.815.622 1.006l.153.076c.433.217.956.132 1.298-.21l.723-.723a8.7 8.7 0 0 0 2.288-4.042 1.087 1.087 0 0 0-.358-1.099l-1.33-1.108c-.251-.21-.582-.299-.905-.245l-1.17.195a1.125 1.125 0 0 1-.98-.314l-.295-.295a1.125 1.125 0 0 1 0-1.591l.13-.132a1.125 1.125 0 0 1 1.3-.21l.603.302a.809.809 0 0 0 1.086-1.086L14.25 7.5l1.256-.837a4.5 4.5 0 0 0 1.528-1.732l.146-.292M6.115 5.19A9 9 0 1 0 17.18 4.64M6.115 5.19A8.965 8.965 0 0 1 12 3c1.929 0 3.716.607 5.18 1.64"
          />
        </svg>

        <h2 className="form-title text-center pt-3">What's Glowing</h2>
      </div>
      <hr className="border-[--primary-color]" />
      {trendingTopics.map((TT) => (
        <button
          key={TT.id}
          onClick={() => handleClick(TT.topic)}
          className="m-5 border-none w-[90%]  cursor-pointer bg-[--secondary-color] hover:bg-[--primary-color] hover:text-[--secondary-color] rounded-lg p-3 duration-[300ms]  text-[--primary-color]"
        >
          <p className="text-xl">#{TT.topic}</p>
          <p className="text-gray-300 mb-3">{TT.count} posts</p>
        </button>
      ))}
    </div>
  );
};

export default Glowing;
