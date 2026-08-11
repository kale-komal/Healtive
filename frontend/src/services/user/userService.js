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


const userService = {

    getUsers,

    getUserById,

};

export default userService;