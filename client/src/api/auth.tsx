import axios from "axios";

export const refreshAccessToken = async () => {
    const response = await axios.post("http://localhost:5000/api/auth/refresh", null, {
        withCredentials: true,
    });

    if (response.status !== 200) {
        throw new Error("Failed to refresh access token");
    }

    return true;
};