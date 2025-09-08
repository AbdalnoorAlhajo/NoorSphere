export const serverUrl = "http://localhost:5141/api/";

export const formatDate = (date) => {
  return new Intl.DateTimeFormat("en", { year: "numeric", month: "long" }).format(new Date(date));
};
