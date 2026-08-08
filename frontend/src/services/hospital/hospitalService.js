import apiClient from "../api/apiClient";


const getHospitals = async (params) => {

    const response = await apiClient.get(

        "/admin/hospitals",

        {

            params,

        }

    );

    return response.data;

};

const getHospitalById = async (hospitalId) => {

    const response = await apiClient.get(
        `/admin/hospitals/${hospitalId}`
    );

    return response.data;

};

const createHospital = async (data) => {

    const response = await apiClient.post(
        "/admin/hospitals",
        data
    );

    return response.data;

};

const updateHospital = async (hospitalId, data) => {

    const response = await apiClient.put(
        `/admin/hospitals/${hospitalId}`,
        data
    );

    return response.data;

};

const deleteHospital = async (hospitalId) => {

    const response = await apiClient.delete(
        `/admin/hospitals/${hospitalId}`
    );

    return response.data;

};

// Activate Hospital
const activateHospital = async (hospitalId) => {

    const response = await apiClient.patch(
        `/admin/hospitals/${hospitalId}/activate`
    );

    return response.data;
};

const deactivateHospital = async (hospitalId) => {

    const response = await apiClient.patch(
        `/admin/hospitals/${hospitalId}/deactivate`
    );

    return response.data;
};


const hospitalService = {
    getHospitals,
    getHospitalById,
    createHospital,
    updateHospital,
    deleteHospital,
    activateHospital,
    deactivateHospital,
};



export default hospitalService;