import apiClient from "../api/apiClient";
import ENDPOINTS from "../api/endpoints";

const getBranches = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.BRANCHES,
        { params }
    );

    return response.data;

};

const getDepartments = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.DEPARTMENTS,
        { params }
    );

    return response.data;

};

const getDoctors = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.DOCTORS,
        { params }
    );

    return response.data;

};

const getToday = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.APPOINTMENTS.LIST,
        { params }
    );

    return response.data;

};

const getProfile = async () => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.PROFILE.GET
    );

    return response.data;

};

const updateProfile = async (data) => {

    const response = await apiClient.put(
        ENDPOINTS.HOSPITAL.PROFILE.UPDATE,
        data
    );

    return response.data;

};

const hospitalAdminService = {
    getBranches,
    getDepartments,
    getDoctors,
    getToday,
    getProfile,
    updateProfile,
};

export default hospitalAdminService;