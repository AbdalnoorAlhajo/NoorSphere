import { createContext, useContext, useState } from "react";
import { jwtDecode } from "jwt-decode";

const TokenContext = createContext();

export const TokenProvider = ({ children }) => {
  const [token, setToken] = useState("");
  const [decoded, setDecoded] = useState(null);

  const saveToken = (token) => {
    setToken(token);
    setDecoded(jwtDecode(token));
    localStorage.setItem("token", token);
  };

  const logout = () => {
    setToken("");
    setDecoded(null);
    localStorage.removeItem("token");
  };

  return <TokenContext.Provider value={{ token, decoded, saveToken, logout }}>{children}</TokenContext.Provider>;
};

export const useToken = () => useContext(TokenContext);
