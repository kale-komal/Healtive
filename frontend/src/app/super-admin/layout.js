"use client";

import ProtectedRoute from "@/components/auth/ProtectedRoute";
import SuperAdminLayout from "@/components/super-admin/SuperAdminLayout";

export default function Layout({ children }) {

    return (

        <ProtectedRoute allowedRoles={["SuperAdmin"]}>

            <SuperAdminLayout>

                {children}

            </SuperAdminLayout>

        </ProtectedRoute>

    );

}