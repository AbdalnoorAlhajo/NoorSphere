import axios from "axios";
import { serverUrl } from "../global";

export const loginUser = (userCredentials) => {
  return axios
    .post(`${serverUrl}Auth/login`, userCredentials, {
      headers: {
        "Content-Type": "application/json",
      },
    })
    .then((response) => {
      if (response.data.token) {
        return response;
      } else {
        throw new Error(response?.errors?.[0]?.msg || "Something went wrong. Please try again.");
      }
    })
    .catch((error) => {
      if (error.response) {
        const status = error.response.status;
        if (status === 400) {
          throw new Error("Bad request. Please check your input.");
        } else if (status === 404) {
          throw new Error("User not found. Please check your credentials.");
        } else if (status === 500) {
          throw new Error("Internal server error. Please try again later.");
        } else {
          throw new Error("Something went wrong. Please try again.");
        }
      } else {
        throw new Error("An unexpected error occurred. Please try again later.");
      }
    });
};

export const registerUser = (userData) => {
  return axios
    .post(`${serverUrl}Auth/register`, userData, {
      headers: {
        "Content-Type": "application/json",
      },
    })
    .then((response) => {
      if (response.data.token) {
        return response;
      } else {
        throw new Error(response?.errors?.[0]?.msg || "Something went wrong. Please try again.");
      }
    })
    .catch((error) => {
      if (error.response) {
        const status = error.response.status;
        if (status === 400) {
          throw new Error("Bad request. Please check your input.");
        } else if (status === 500) {
          throw new Error("Internal server error. Please try again later.");
        } else {
          throw new Error("Something went wrong. Please try again.");
        }
      } else {
        throw new Error("An unexpected error occurred. Please try again later.");
      }
    });
};

export const getProfiles = (token) => {
  return axios
    .get(`${serverUrl}profiles`, {
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

export const getProfileById = (id, token) => {
  return axios
    .get(`${serverUrl}profiles/${id}`, {
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

export const deleteUserAccount = (token) => {
  return axios
    .delete(`${serverUrl}api/users`, {
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

export const GetFollowingSuggestions = async (token) => {
  return axios
    .get(`${serverUrl}profiles/suggestions`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      throw error;
    });
};

export const addFollow = (token, addNewFollow) => {
  console.log("users ids in following", addNewFollow); //debbuggin

  return axios
    .post(`${serverUrl}users/follows`, addNewFollow, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    })
    .then((response) => response.data)
    .catch((error) => {
      console.log("Error from addFollow", error); //debbugging
      throw error;
    });
};
