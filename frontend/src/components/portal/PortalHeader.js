"use client";

import { Bell, Search, LogOut } from "lucide-react";
import { useAuth } from "@/contexts/AuthContext";
import { useRouter } from "next/navigation";

export default function PortalHeader() {

    const router = useRouter();

    const { user, logout } = useAuth();

    const handleLogout = () => {

        logout();

        router.push("/login");

    };

    return (

        <header className="portal-header">

            <div className="portal-header-search">

                <Search size={18} />

                <input
                    type="text"
                    placeholder="Search..."
                />

            </div>

            <div className="portal-header-right">

                <button className="portal-notification-btn">

                    <Bell size={20} />

                </button>

                <div className="portal-profile">

                    <div className="portal-profile-avatar">

                        {user?.fullName?.charAt(0) || "D"}

                    </div>

                    <div className="portal-profile-info">

                        <h5>{user?.fullName}</h5>

                        <span>{user?.role}</span>

                    </div>

                </div>

                <button
                    className="portal-logout-btn"
                    onClick={handleLogout}
                >

                    <LogOut size={18} />

                    Logout

                </button>

            </div>

        </header>

    );

}