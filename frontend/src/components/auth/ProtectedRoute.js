"use client";

import { useEffect } from "react";
import { useRouter } from "next/navigation";

import { useAuth } from "@/contexts/AuthContext";

export default function ProtectedRoute({
    children,
    allowedRoles = [],
}) {

    const router = useRouter();

    const { user, token, loading } = useAuth();

    useEffect(() => {

        // Wait until auth context finishes loading
        if (loading) return;

        // Not logged in
        if (!token || !user) {

            router.replace("/login");

            return;

        }

        // Role not allowed
        if (
            allowedRoles.length > 0 &&
            !allowedRoles.includes(user.role)
        ) {

            router.replace("/login");

            return;

        }

    }, [loading, token, user, allowedRoles, router]);

    // Prevent page flash while checking auth
    if (loading) {

        return (
            <div
                style={{
                    minHeight: "100vh",
                    display: "flex",
                    justifyContent: "center",
                    alignItems: "center",
                    fontSize: "18px",
                    fontWeight: "500",
                }}
            >
                Loading...
            </div>
        );

    }

    // While redirecting
    if (!token || !user) {

        return null;

    }

    // Wrong role
    if (
        allowedRoles.length > 0 &&
        !allowedRoles.includes(user.role)
    ) {

        return null;

    }

    return children;

}