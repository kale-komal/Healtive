"use client";

import { createContext, useContext, useEffect, useState } from "react";

import {
    saveAuth,
    getUser,
    getToken,
    logout as clearAuth,
} from "@/services/auth/authStorage";

const AuthContext = createContext();

export function AuthProvider({ children }) {
 const [user, setUser] = useState(null);
    const [token, setToken] = useState(null);
    const [loading, setLoading] = useState(true);
    

    useEffect(() => {

    const currentUser = getUser();
    const currentToken = getToken();

    if (currentUser) {
        setUser(currentUser);
    }

    if (currentToken) {
        setToken(currentToken);
    }

    setLoading(false);

}, []);

    const login = (data) => {

    saveAuth(data);

    setUser(data.user);
    setToken(data.accessToken);

};

    const logout = () => {

    clearAuth();

    setUser(null);
    setToken(null);

};

    return (

        <AuthContext.Provider
    value={{
        user,
        token,
        loading,
        login,
        logout,
        isAuthenticated: !!user,
    }}
>

            {children}

        </AuthContext.Provider>

    );

}

export function useAuth() {

    return useContext(AuthContext);

}