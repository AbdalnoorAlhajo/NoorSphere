import axios from "axios";

export const UploadImageToCloudinary = async (file) => {
  const data = new FormData();
  data.append("file", file);
  data.append("upload_preset", "NoorShpere");
  data.append("cloud_name", "dmkmb9ez0");

  try {
    const response = await axios.post("https://api.cloudinary.com/v1_1/dmkmb9ez0/image/upload", data);
    return response.data.secure_url;
  } catch (error) {
    console.error("Cloudinary upload failed", error);
  }
};
