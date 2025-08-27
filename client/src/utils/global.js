export const serverUrl = "";

export const formatDate = (date) => {
  return new Intl.DateTimeFormat("en", { year: "numeric", month: "long" }).format(new Date(date));
};
