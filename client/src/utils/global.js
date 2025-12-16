export const serverUrl = "https://app-noorshpere-swedencentral-dev-001-budggdf4ayb6cuck.swedencentral-01.azurewebsites.net/api/";
export const formatDate = (date) => {
  return new Intl.DateTimeFormat("en", { year: "numeric", month: "long" }).format(new Date(date));
};
