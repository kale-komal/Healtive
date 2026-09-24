"use client";

import {
    LayoutDashboard,
    CalendarDays,
    ListOrdered,
    ClipboardCheck,
    Users,
} from "lucide-react";

import ProtectedRoute from "@/components/auth/ProtectedRoute";
import PortalLayout from "@/components/portal/PortalLayout";

const receptionistMenu = [
    {
        title: "Dashboard",
        href: "/receptionist/dashboard",
        icon: LayoutDashboard,
    },
    {
        title: "Today's Appointments",
        href: "/receptionist/today-appointments",
        icon: CalendarDays,
        disabled: true,
    },
    {
        title: "Queue",
        href: "/receptionist/queue",
        icon: ListOrdered,
        disabled: true,
    },
    {
        title: "Check-In",
        href: "/receptionist/check-in",
        icon: ClipboardCheck,
        disabled: true,
    },
    {
        title: "Patients",
        href: "/receptionist/patients",
        icon: Users,
        disabled: true,
    },
];

export default function ReceptionistLayout({ children }) {

    return (

        <ProtectedRoute allowedRoles={["Receptionist"]}>

            <PortalLayout
                brand="Healtive"
                roleLabel="Receptionist Portal"
                menuItems={receptionistMenu}
            >

                {children}

            </PortalLayout>

        </ProtectedRoute>

    );

}