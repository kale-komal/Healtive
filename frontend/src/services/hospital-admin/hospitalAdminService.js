import apiClient from "../api/apiClient";
import ENDPOINTS from "../api/endpoints";

const getBranches = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.BRANCHES.LIST,
        { params }
    );

    return response.data;

};

const getBranchById = async (id) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.BRANCHES.GET_BY_ID(id)
    );

    return response.data;

};

const createBranch = async (data) => {

    const response = await apiClient.post(
        ENDPOINTS.HOSPITAL.BRANCHES.CREATE,
        data
    );

    return response.data;

};

const updateBranch = async (id, data) => {

    const response = await apiClient.put(
        ENDPOINTS.HOSPITAL.BRANCHES.UPDATE(id),
        data
    );

    return response.data;

};

const deleteBranch = async (id) => {

    const response = await apiClient.delete(
        ENDPOINTS.HOSPITAL.BRANCHES.DELETE(id)
    );

    return response.data;

};

const activateBranch = async (id) => {

    const response = await apiClient.patch(
        ENDPOINTS.HOSPITAL.BRANCHES.ACTIVATE(id)
    );

    return response.data;

};

const deactivateBranch = async (id) => {

    const response = await apiClient.patch(
        ENDPOINTS.HOSPITAL.BRANCHES.DEACTIVATE(id)
    );

    return response.data;

};

const getDepartments = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.DEPARTMENTS.LIST,
        { params }
    );

    return response.data;

};

const getDepartmentById = async (id) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.DEPARTMENTS.GET_BY_ID(id)
    );

    return response.data;

};

const createDepartment = async (data) => {

    const response = await apiClient.post(
        ENDPOINTS.HOSPITAL.DEPARTMENTS.CREATE,
        data
    );

    return response.data;

};

const updateDepartment = async (id, data) => {

    const response = await apiClient.put(
        ENDPOINTS.HOSPITAL.DEPARTMENTS.UPDATE(id),
        data
    );

    return response.data;

};

const deleteDepartment = async (id) => {

    const response = await apiClient.delete(
        ENDPOINTS.HOSPITAL.DEPARTMENTS.DELETE(id)
    );

    return response.data;

};

const activateDepartment = async (id) => {

    const response = await apiClient.patch(
        ENDPOINTS.HOSPITAL.DEPARTMENTS.ACTIVATE(id)
    );

    return response.data;

};

const deactivateDepartment = async (id) => {

    const response = await apiClient.patch(
        ENDPOINTS.HOSPITAL.DEPARTMENTS.DEACTIVATE(id)
    );

    return response.data;

};

const getDoctors = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.DOCTORS.LIST,
        { params }
    );

    return response.data;

};

const getDoctorById = async (id) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.DOCTORS.GET_BY_ID(id)
    );

    return response.data;

};

const createDoctor = async (data) => {

    const response = await apiClient.post(
        ENDPOINTS.HOSPITAL.DOCTORS.CREATE,
        data
    );

    return response.data;

};

const updateDoctor = async (id, data) => {

    const response = await apiClient.put(
        ENDPOINTS.HOSPITAL.DOCTORS.UPDATE(id),
        data
    );

    return response.data;

};

const deleteDoctor = async (id) => {

    const response = await apiClient.delete(
        ENDPOINTS.HOSPITAL.DOCTORS.DELETE(id)
    );

    return response.data;

};

const activateDoctor = async (id) => {

    const response = await apiClient.patch(
        ENDPOINTS.HOSPITAL.DOCTORS.ACTIVATE(id)
    );

    return response.data;

};

const deactivateDoctor = async (id) => {

    const response = await apiClient.patch(
        ENDPOINTS.HOSPITAL.DOCTORS.DEACTIVATE(id)
    );

    return response.data;

};

const resetDoctorPassword = async (id) => {

    const response = await apiClient.post(
        ENDPOINTS.HOSPITAL.DOCTORS.RESET_PASSWORD(id)
    );

    return response.data;

};

const getRoles = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.ROLES.LIST,
        { params }
    );

    return response.data;

};

const getStaff = async (params) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.STAFF.LIST,
        { params }
    );

    return response.data;

};

const getStaffById = async (id) => {

    const response = await apiClient.get(
        ENDPOINTS.HOSPITAL.STAFF.GET_BY_ID(id)
    );

    return response.data;

};

const createStaff = async (data) => {

    const response = await apiClient.post(
        ENDPOINTS.HOSPITAL.STAFF.CREATE,
        data
    );

    return response.data;

};

const updateStaff = async (id, data) => {

    const response = await apiClient.put(
        ENDPOINTS.HOSPITAL.STAFF.UPDATE(id),
        data
    );

    return response.data;

};

const deleteStaff = async (id) => {

    const response = await apiClient.delete(
        ENDPOINTS.HOSPITAL.STAFF.DELETE(id)
    );

    return response.data;

};

const activateStaff = async (id) => {

    const response = await apiClient.patch(
        ENDPOINTS.HOSPITAL.STAFF.ACTIVATE(id)
    );

    return response.data;

};

const deactivateStaff = async (id) => {

    const response = await apiClient.patch(
        ENDPOINTS.HOSPITAL.STAFF.DEACTIVATE(id)
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
    getBranchById,
    createBranch,
    updateBranch,
    deleteBranch,
    activateBranch,
    deactivateBranch,
    getDepartments,
    getDepartmentById,
    createDepartment,
    updateDepartment,
    deleteDepartment,
    activateDepartment,
    deactivateDepartment,
    getDoctors,
    getDoctorById,
    createDoctor,
    updateDoctor,
    deleteDoctor,
    activateDoctor,
    deactivateDoctor,
    resetDoctorPassword,
    getRoles,
    getStaff,
    getStaffById,
    createStaff,
    updateStaff,
    deleteStaff,
    activateStaff,
    deactivateStaff,
    getToday,
    getProfile,
    updateProfile,
};

export default hospitalAdminService;