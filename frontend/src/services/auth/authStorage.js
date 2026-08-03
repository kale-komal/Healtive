const TOKEN_KEY = "healtive_access_token";
const REFRESH_KEY = "healtive_refresh_token";
const USER_KEY = "healtive_user";

export const saveAuth = (data) => {
  localStorage.setItem(TOKEN_KEY, data.accessToken);
  localStorage.setItem(REFRESH_KEY, data.refreshToken);
  localStorage.setItem(USER_KEY, JSON.stringify(data.user));
};

export const getToken = () => {
  return localStorage.getItem(TOKEN_KEY);
};

export const getRefreshToken = () => {
  return localStorage.getItem(REFRESH_KEY);
};

export const getUser = () => {
  const user = localStorage.getItem(USER_KEY);

  return user ? JSON.parse(user) : null;
};

export const logout = () => {
  localStorage.removeItem(TOKEN_KEY);
  localStorage.removeItem(REFRESH_KEY);
  localStorage.removeItem(USER_KEY);
};