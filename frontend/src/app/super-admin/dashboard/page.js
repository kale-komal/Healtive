"use client";

import ProtectedRoute from "@/components/auth/ProtectedRoute";

export default function SuperAdminDashboard() {
    return (
        <ProtectedRoute allowedRoles={["SuperAdmin"]}>
            <div style={{ padding: "40px" }}>
                <h1>Super Admin Dashboard</h1>
                <p>Welcome to Healtive Admin Panel</p>
            </div>
        </ProtectedRoute>
    );
}