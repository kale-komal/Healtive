import apiClient from "../api/apiClient";
import ENDPOINTS from "../api/endpoints";

const login = async (data) => {
  const response = await apiClient.post(
    ENDPOINTS.AUTH.LOGIN,
    data
  );

  return response.data;
};

const authService = {
  login,
};

export default authService;