import axios from "axios";
import { serverUrl } from "../global";

export const GetAllPosts = (token) => {
  return axios
    .get(`${serverUrl}posts/all`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      handleError(error);
    });
};

export const GetPostById = (token, postId) => {
  return axios
    .get(`${serverUrl}posts/${postId}`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    })
    .then((response) => response.data)
    .catch((error) => {
      handleError(error);
    });
};

export const AddPost = (token, postData) => {
  axios
    .post(`${serverUrl}posts`, postData, {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
    })
    .then((response) => {
      return response;
    })
    .catch((error) => {
      handleError(error);
    });
};

export const LikePost = async (token, postId) => {
  return await axios
    .post(`${serverUrl}posts/like/${postId}`, null, {
      headers: {
        "Content-Type": "application/json",
        Authorization: `Bearer ${token}`,
      },
    })
    .then((response) => response)
    .catch((error) => {
      throw error;
    });
};

export const SavePost = async (token, requestBody) => {
  return axios
    .post(`${serverUrl}posts`, requestBody, {
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

export const GetAllComments = async (token, postId) => {
  return axios
    .get(`${serverUrl}posts/comments/${postId}`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
    })
    .then((response) => {
      return response.data;
    })
    .catch((error) => {
      console.log("error from GetAllComments", error);
      handleError(error);
    });
};

export const GetTrendingTopics = async (token) => {
  return axios
    .get(`${serverUrl}posts/trending`, {
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

export const GetPostsByText = async (token, text) => {
  return axios
    .get(`${serverUrl}posts/search`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
      params: { text },
    })
    .then((response) => response.data)
    .catch((error) => {
      handleError(error);
    });
};

export const GetPostsByUserId = async (token, userId) => {
  return axios
    .get(`${serverUrl}posts/user`, {
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

export const GetPostsByFollowedUsers = async (token) => {
  return axios
    .get(`${serverUrl}posts/following`, {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
    })
    .then((response) => response.data)
    .catch((error) => {
      handleError(error);
    });
};

export const GetPostsLikedByUser = async (token, userId) => {
  return await axios
    .get(`${serverUrl}posts/liked`, {
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

export const GeneratePostWithAI = async (token, post) => {
  return axios
    .post(`${serverUrl}posts/generatePost`, post, {
      headers: {
        "Content-Type": "application/json",
        Authorization: "Bearer " + token,
      },
    })
    .then((response) => {
      return response.data.candidates[0].content.parts[0].text;
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
