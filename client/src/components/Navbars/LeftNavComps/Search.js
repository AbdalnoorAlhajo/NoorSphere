import { MagnifyingGlassIcon } from "@heroicons/react/24/outline";
import { useState } from "react";
import { useNavigate } from "react-router-dom";

const Search = ({ type }) => {
  const [query, setQuery] = useState("");
  const navigate = useNavigate();

  const handleSubmit = (e) => {
    e.preventDefault();

    const queryTrimed = query.trim();

    if (!queryTrimed) return;

    if (type === "explore") navigate(`/explore?query=${encodeURIComponent(queryTrimed)}`);
    else if (type === "developers") navigate(`/developers?query=${encodeURIComponent(queryTrimed)}`);
  };

  return (
    <form onSubmit={handleSubmit} className="flex items-center bg-[--primary-color] p-5 rounded-xl my-5">
      <button type="submit" className="bg-inherit border-none">
        <MagnifyingGlassIcon className="w-5 h-5 text-white cursor-pointer" />
      </button>
      <input
        type="text"
        placeholder="Search"
        className="ml-2 bg-transparent outline-none border-none text-white placeholder-white w-full"
        value={query}
        onChange={(e) => setQuery(e.target.value)}
      />
    </form>
  );
};

export default Search;
