import apiClient from "../api/apiClient";

const getSubscriptionPlans = async () => {

    const response = await apiClient.get(
        "/admin/subscription-plans"
    );

    return response.data;
};


const getSubscriptionPlanById = async (id) => {

    const response = await apiClient.get(
        `/admin/subscription-plans/${id}`
    );

    return response.data;
};


const createSubscriptionPlan = async (data) => {

    const response = await apiClient.post(
        "/admin/subscription-plans",
        data
    );

    return response.data;
};


const updateSubscriptionPlan = async (id, data) => {

    const response = await apiClient.put(
        `/admin/subscription-plans/${id}`,
        data
    );

    return response.data;
};


const deleteSubscriptionPlan = async (id) => {

    const response = await apiClient.delete(
        `/admin/subscription-plans/${id}`
    );

    return response.data;
};


const subscriptionPlanService = {

    getSubscriptionPlans,

    getSubscriptionPlanById,

    createSubscriptionPlan,

    updateSubscriptionPlan,

    deleteSubscriptionPlan,

};


export default subscriptionPlanService;