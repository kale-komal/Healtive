"use client";

import Sidebar from "./Sidebar";
import Header from "./Header";

import "./SuperAdminLayout.css";

export default function SuperAdminLayout({ children }) {

    return (

        <div className="admin-layout">

            <Sidebar />

            <div className="admin-main">

                <Header />

                <div className="admin-content">

                    {children}

                </div>

            </div>

        </div>

    );

}