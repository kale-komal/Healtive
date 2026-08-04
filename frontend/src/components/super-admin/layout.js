"use client";

import ProtectedRoute from "@/components/auth/ProtectedRoute";

import Sidebar from "@/components/super-admin/Sidebar";
import Header from "@/components/super-admin/Header";

import "@/components/super-admin/SuperAdminLayout.css";

export default function SuperAdminLayout({ children }) {
    return (
        <ProtectedRoute allowedRoles={["SuperAdmin"]}>

            <div className="admin-layout">

                <Sidebar />

                <div className="admin-content">

                    <Header />

                    <main className="admin-main">

                        {children}

                    </main>

                </div>

            </div>

        </ProtectedRoute>
    );
}