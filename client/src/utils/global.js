export const serverUrl = "https://localhost:7218/api/";

export const formatDate = (date) => {
  return new Intl.DateTimeFormat("en", { year: "numeric", month: "long" }).format(new Date(date));
};
