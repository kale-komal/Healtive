"use client";

import { Bell, Search, LogOut } from "lucide-react";
import { useAuth } from "@/contexts/AuthContext";
import { useRouter } from "next/navigation";

import "./Header.css";

export default function Header() {

    const router = useRouter();

    const { user, logout } = useAuth();

    const handleLogout = () => {

        logout();

        router.push("/login");

    };

    return (

        <header className="admin-header">

            <div className="header-search">

                <Search size={18} />

                <input
                    type="text"
                    placeholder="Search..."
                />

            </div>

            <div className="header-right">

                <button className="notification-btn">

                    <Bell size={20} />

                </button>

                <div className="admin-profile">

                    <div className="profile-avatar">

                        {user?.fullName?.charAt(0) || "A"}

                    </div>

                    <div className="profile-info">

                        <h5>{user?.fullName}</h5>

                        <span>{user?.role}</span>

                    </div>

                </div>

                <button
                    className="logout-btn"
                    onClick={handleLogout}
                >

                    <LogOut size={18} />

                    Logout

                </button>

            </div>

        </header>

    );

}