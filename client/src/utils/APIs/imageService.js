import axios from "axios";

export const UploadImageToCloudinary = async (file) => {
  const data = new FormData();
  data.append("file", file);
  data.append("upload_preset", "upload_preset");
  data.append("cloud_name", "cloud_name");

  try {
    const response = await axios.post("https://api.cloudinary.com/v1_1/cloud_name/image/upload", data);
    return response.data.secure_url;
  } catch (error) {
    console.error("Cloudinary upload failed", error);
  }
};
