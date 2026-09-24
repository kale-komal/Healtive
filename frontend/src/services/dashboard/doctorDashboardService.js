import apiClient from "../api/apiClient";
import ENDPOINTS from "../api/endpoints";

const getDashboard = async () => {

    const response = await apiClient.get(
        ENDPOINTS.DOCTOR_DASHBOARD.GET
    );

    return response.data;

};

const getTodayAppointments = async () => {

    const response = await apiClient.get(
        ENDPOINTS.DOCTOR_DASHBOARD.TODAY_APPOINTMENTS
    );

    return response.data;

};

const doctorDashboardService = {
    getDashboard,
    getTodayAppointments,
};

export default doctorDashboardService;