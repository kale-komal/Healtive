import apiClient from "../api/apiClient";

const getUsers = async () => {

    const response = await apiClient.get(
        "/admin/users"
    );

    return response.data;
};


const getUserById = async (id) => {

    const response = await apiClient.get(
        `/admin/users/${id}`
    );

    return response.data;
};

const getProfile = async () => {

    const response = await apiClient.get(
        "/admin/profile"
    );

    return response.data;
};

const changePassword = async (data) => {

    const response = await apiClient.patch(
        "/Auth/change-password",
        data
    );

    return response.data;
};

const userService = {

    getUsers,

    getUserById,

    getProfile,

    changePassword,

};

export default userService;