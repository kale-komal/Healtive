import apiClient from "../api/apiClient";

const getHospitals = async () => {

    const response = await apiClient.get(
        "/admin/hospitals"
    );

    return response.data;

};

const hospitalService = {
    getHospitals,
};

export default hospitalService;