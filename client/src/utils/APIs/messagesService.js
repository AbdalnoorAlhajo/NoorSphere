import axios from "axios";
import { serverUrl } from "../global";

export const ChatWithAIapi = async (token, message) => {
  const payload = {
    contents: message,
  };

  console.log("Payload:", payload);
  return axios
    .post(`${serverUrl}messages/ChatWithAI`, payload, {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      handleError(error);
    });
};

const handleError = (error) => {
  if (error.response) {
    if (error.response.status === 404) {
      throw new Error("No post found.");
    } else if (error.response.status === 401) {
      throw new Error("Unauthorized");
    } else if (error.response.status === 400) {
      throw new Error("Bad request. Please check your input.");
    } else {
      throw new Error("Something went wrong. Please try again.");
    }
  } else {
    console.error("Error:", error.message);
    throw new Error("An unexpected error occurred. Please try again later.");
  }
};
