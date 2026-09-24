import apiClient from "../api/apiClient";
import ENDPOINTS from "../api/endpoints";

const getToday = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.APPOINTMENTS.LIST,
        { params }
    );

    return response.data;

};

const receptionistAppointmentService = {
    getToday,
};

export default receptionistAppointmentService;