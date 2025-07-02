import { useState, useEffect } from "react";
import { useToken } from "../../TokenContext";
import { GetFollowingSuggestions } from "../../../utils/APIs/userService";
import SuggestedPerson from "./SuggestedPerson";

const Following = () => {
  const [followingSuggestions, setFollowingSuggestions] = useState([]);
  const { token } = useToken();

  useEffect(() => {
    const fetchFollowingSuggestions = async () => {
      try {
        const followingSuggestions = await GetFollowingSuggestions(token);
        setFollowingSuggestions(followingSuggestions);
      } catch (error) {
        console.log(error);
      }
    };
    fetchFollowingSuggestions();
  }, [token]);

  return (
    <div className="border-2 border-solid rounded-xl border-[--primary-color]">
      <div className="flex items-center p-3">
        <svg
          xmlns="http://www.w3.org/2000/svg"
          fill="none"
          viewBox="0 0 24 24"
          strokeWidth={1.5}
          stroke="currentColor"
          className="size-6 mr-6"
          style={{ color: "var(--primary-color)" }}
        >
          <path
            strokeLinecap="round"
            strokeLinejoin="round"
            d="M15 19.128a9.38 9.38 0 0 0 2.625.372 9.337 9.337 0 0 0 4.121-.952 4.125 4.125 0 0 0-7.533-2.493M15 19.128v-.003c0-1.113-.285-2.16-.786-3.07M15 19.128v.106A12.318 12.318 0 0 1 8.624 21c-2.331 0-4.512-.645-6.374-1.766l-.001-.109a6.375 6.375 0 0 1 11.964-3.07M12 6.375a3.375 3.375 0 1 1-6.75 0 3.375 3.375 0 0 1 6.75 0Zm8.25 2.25a2.625 2.625 0 1 1-5.25 0 2.625 2.625 0 0 1 5.25 0Z"
          />
        </svg>

        <h2 className="form-title text-center pt-3">Who to follow</h2>
      </div>
      <hr className="border-[--primary-color]" />
      {followingSuggestions.length === 0 ? (
        <h2 className="text-center text-gray-300 mt-5">No suggestions avaliable</h2>
      ) : (
        followingSuggestions.map((user) => (
          <div key={user.userId} className="flex items-center m-5 p-3">
            <SuggestedPerson user={user} />
          </div>
        ))
      )}
    </div>
  );
};

export default Following;
