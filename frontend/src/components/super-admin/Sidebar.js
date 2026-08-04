"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

import {
    LayoutDashboard,
    Building2,
    Users,
    CreditCard,
    BadgeIndianRupee,
    Settings,
    UserCircle,
} from "lucide-react";

import "./Sidebar.css";

const menuItems = [
    {
        title: "Dashboard",
        href: "/super-admin/dashboard",
        icon: LayoutDashboard,
    },
    {
        title: "Hospitals",
        href: "/super-admin/hospitals",
        icon: Building2,
    },
    {
        title: "Users",
        href: "/super-admin/users",
        icon: Users,
    },
    {
        title: "Subscriptions",
        href: "/super-admin/subscriptions",
        icon: CreditCard,
    },
    {
        title: "Plans",
        href: "/super-admin/plans",
        icon: BadgeIndianRupee,
    },
    {
        title: "Settings",
        href: "/super-admin/settings",
        icon: Settings,
    },
    {
        title: "Profile",
        href: "/super-admin/profile",
        icon: UserCircle,
    },
];

export default function Sidebar() {

    const pathname = usePathname();

    return (

        <aside className="admin-sidebar">

            <div className="sidebar-logo">

                <h3>Healtive</h3>

                <span>Super Admin</span>

            </div>

            <nav className="sidebar-menu">

                {
                    menuItems.map((item) => {

                        const Icon = item.icon;

                        return (

                            <Link
                                key={item.href}
                                href={item.href}
                                className={
                                    pathname === item.href
                                        ? "sidebar-link active"
                                        : "sidebar-link"
                                }
                            >

                                <Icon size={20} />

                                <span>{item.title}</span>

                            </Link>

                        );

                    })
                }

            </nav>

        </aside>

    );

}