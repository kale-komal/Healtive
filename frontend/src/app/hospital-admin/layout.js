"use client";

import {
    LayoutDashboard,
    Building2,
    Layers,
    Stethoscope,
    Briefcase,
    ShieldCheck,
    CalendarDays,
    Users,
    Hospital,
} from "lucide-react";

import ProtectedRoute from "@/components/auth/ProtectedRoute";
import PortalLayout from "@/components/portal/PortalLayout";

const hospitalAdminMenu = [
    {
        title: "Dashboard",
        href: "/hospital-admin/dashboard",
        icon: LayoutDashboard,
    },
    {
        title: "Profile",
        href: "/hospital-admin/profile",
        icon: Hospital,
    },
    {
        title: "Branches",
        href: "/hospital-admin/branches",
        icon: Building2,
        disabled: true,
    },
    {
        title: "Departments",
        href: "/hospital-admin/departments",
        icon: Layers,
        disabled: true,
    },
    {
        title: "Doctors",
        href: "/hospital-admin/doctors",
        icon: Stethoscope,
        disabled: true,
    },
    {
        title: "Staff",
        href: "/hospital-admin/staff",
        icon: Briefcase,
        disabled: true,
    },
    {
        title: "Roles",
        href: "/hospital-admin/roles",
        icon: ShieldCheck,
        disabled: true,
    },
    {
        title: "Appointments",
        href: "/hospital-admin/appointments",
        icon: CalendarDays,
        disabled: true,
    },
    {
        title: "Patients",
        href: "/hospital-admin/patients",
        icon: Users,
        disabled: true,
    },
];

export default function HospitalAdminLayout({ children }) {

    return (

        <ProtectedRoute allowedRoles={["HospitalAdmin"]}>

            <PortalLayout
                brand="Healtive"
                roleLabel="Hospital Admin Portal"
                menuItems={hospitalAdminMenu}
            >

                {children}

            </PortalLayout>

        </ProtectedRoute>

    );

}