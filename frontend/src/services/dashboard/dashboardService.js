import apiClient from "../api/apiClient";

const getDashboard = async () => {

    const response = await apiClient.get(
        "/admin/dashboard"
    );

    return response.data;

};

const dashboardService = {
    getDashboard,
};

export default dashboardService;