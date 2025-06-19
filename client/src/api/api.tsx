import axios from "axios";
import {refreshAccessToken} from "./auth";

const api = axios.create({
    baseURL: "http://localhost:5001/api",
    withCredentials: true,
});

api.interceptors.response.use(
    response => response,
    async (error) => {
        const originalRequest = error.config;

        if (error.response?.status === 401 && !originalRequest._retry) {
            originalRequest._retry = true;

            try {
                await refreshAccessToken();
                return api(originalRequest);
            } catch (err) {
                localStorage.clear();
                window.location.href = "/login";
                return Promise.reject(err);
            }
        }

        return Promise.reject(error);
    }
);

export default api;
