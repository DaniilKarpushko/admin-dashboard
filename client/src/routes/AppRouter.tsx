import {Navigate, Route, Router, Routes} from "react-router-dom";
import LoginPage from "../pages/LoginPage";
import DashboardPage from "../pages/DashboardPage";
import PrivateRoute from "./PrivateRoute";

const AppRouter = () => {
    return (
        <Routes>
            <Route path="/login" element={<LoginPage/>}/>
            <Route
                path="/dashboard"
                element={
                    <DashboardPage/>
                }
            />
            <Route path="*" element={<Navigate to="/login"/>}/>
        </Routes>
    );
};

export default AppRouter;