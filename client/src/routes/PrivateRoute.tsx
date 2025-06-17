import { Navigate } from "react-router-dom";
import {JSX} from "react";

const PrivateRoute = ({ children }: { children: JSX.Element }) => {
    const token = localStorage.getItem("access_token");

    return token ? children : <Navigate to="/login" replace />;
};

export default PrivateRoute;