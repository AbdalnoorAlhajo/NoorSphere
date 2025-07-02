import axios from "axios";
import { serverUrl } from "../global";

export const getMyProfile = async (token) => {
  return axios
    .get(`${serverUrl}profiles/me`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    })
    .then((response) => response.data)
    .catch((error) => {
      throw error;
    });
};

export const GetFollowingSuggestionsByName = async (token, name) => {
  return axios
    .get(`${serverUrl}profiles/suggestions/search`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
      params: { name },
    })
    .then((response) => response.data)
    .catch((error) => {
      handleError(error);
    });
};

export const GetFollowersAndFollowing = async (token, userId) => {
  return await axios
    .get(`${serverUrl}users/FollowersAndFollwoing`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
      params: { userId },
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      handleError(error);
    });
};

export const GetAvatarURL = async (token) => {
  return await axios
    .get(`${serverUrl}profiles/avatarURL`, {
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
      throw new Error("Not found");
    } else if (error.response.status === 401) {
      throw new Error("Unauthorized");
    } else if (error.response.status === 400) {
      throw new Error("Bad request. Please check your input.");
    } else {
      throw new Error("Something went wrong. Please try again.");
    }
  } else {
    throw new Error("An unexpected error occurred. Please try again later.");
  }
};
