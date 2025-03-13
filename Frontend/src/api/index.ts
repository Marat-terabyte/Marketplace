import axios from "axios";

axios.defaults.withCredentials = true;
const $host = axios.create({
  baseURL: import.meta.env.VITE_BACKEND_HOST,
});

export { $host };
