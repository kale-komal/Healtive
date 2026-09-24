"use client";

import {
    LayoutDashboard,
    CalendarDays,
    ListOrdered,
    Users,
} from "lucide-react";

import ProtectedRoute from "@/components/auth/ProtectedRoute";
import PortalLayout from "@/components/portal/PortalLayout";

const doctorMenu = [
    {
        title: "Dashboard",
        href: "/doctor/dashboard",
        icon: LayoutDashboard,
    },
    {
        title: "Today's Appointments",
        href: "/doctor/today-appointments",
        icon: CalendarDays,
        disabled: true,
    },
    {
        title: "Queue",
        href: "/doctor/queue",
        icon: ListOrdered,
        disabled: true,
    },
    {
        title: "Patients",
        href: "/doctor/patients",
        icon: Users,
        disabled: true,
    },
];

export default function DoctorLayout({ children }) {

    return (

        <ProtectedRoute allowedRoles={["Doctor"]}>

            <PortalLayout
                brand="Healtive"
                menuItems={doctorMenu}
            >

                {children}

            </PortalLayout>

        </ProtectedRoute>

    );

}