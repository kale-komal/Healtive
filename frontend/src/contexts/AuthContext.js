"use client";

import { createContext, useContext, useEffect, useState } from "react";

import {
    saveAuth,
    getUser,
    logout as clearAuth,
} from "@/services/auth/authStorage";

const AuthContext = createContext();

export function AuthProvider({ children }) {

    const [user, setUser] = useState(null);

    const [loading, setLoading] = useState(true);

    useEffect(() => {

        const currentUser = getUser();

        if (currentUser) {

            setUser(currentUser);

        }

        setLoading(false);

    }, []);

    const login = (data) => {

        saveAuth(data);

        setUser(data.user);

    };

    const logout = () => {

        clearAuth();

        setUser(null);

    };

    return (

        <AuthContext.Provider
            value={{
                user,
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