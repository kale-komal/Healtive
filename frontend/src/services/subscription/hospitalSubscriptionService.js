import apiClient from "../api/apiClient";


const getSubscriptions = async () => {

    const response = await apiClient.get(
        "/admin/subscriptions"
    );

    return response.data;
};


const getSubscriptionById = async (id) => {

    const response = await apiClient.get(
        `/admin/subscriptions/${id}`
    );

    return response.data;
};


const createSubscription = async (data) => {

    const response = await apiClient.post(
        "/admin/subscriptions",
        data
    );

    return response.data;
};


const updateSubscription = async (id, data) => {

    const response = await apiClient.put(
        `/admin/subscriptions/${id}`,
        data
    );

    return response.data;
};


const deleteSubscription = async (id) => {

    const response = await apiClient.delete(
        `/admin/subscriptions/${id}`
    );

    return response.data;
};


const renewSubscription = async (id) => {

    const response = await apiClient.patch(
        `/admin/subscriptions/${id}/renew`
    );

    return response.data;
};


const cancelSubscription = async (id) => {

    const response = await apiClient.patch(
        `/admin/subscriptions/${id}/cancel`
    );

    return response.data;
};


const hospitalSubscriptionService = {

    getSubscriptions,

    getSubscriptionById,

    createSubscription,

    updateSubscription,

    deleteSubscription,

    renewSubscription,

    cancelSubscription,

};


export default hospitalSubscriptionService;