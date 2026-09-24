"use client";

import { useEffect, useState } from "react";

import {
    CalendarDays,
    CheckCircle2,
    Clock,
    CalendarClock,
    CalendarX2,
} from "lucide-react";

import { toast } from "react-toastify";

import doctorDashboardService from "@/services/dashboard/doctorDashboardService";
import DashboardSection from "@/components/super-admin/DashboardSection";
import StatCard from "@/components/super-admin/StatCard";
import PageHeader from "@/components/super-admin/PageHeader";
import StatusBadge from "@/components/portal/StatusBadge";
import { useAuth } from "@/contexts/AuthContext";

import "./Dashboard.css";

const normalizeResponse = (data) => {

    // Backend may return the raw object/array, or the
    // { success, message, data, errors } envelope.
    if (
        data &&
        typeof data === "object" &&
        !Array.isArray(data) &&
        "success" in data
    ) {

        return {
            ok: !!data.success,
            message: data.message,
            data: data.success ? data.data : null,
        };

    }

    return {
        ok: data !== null && data !== undefined,
        message: "",
        data,
    };

};

const formatTime = (value) =>
    typeof value === "string" && value.includes(":")
        ? value.slice(0, 5)
        : value;

export default function DoctorDashboardPage() {

    const { user } = useAuth();

    const [dashboard, setDashboard] = useState(null);

    const [appointments, setAppointments] = useState([]);

    const [loading, setLoading] = useState(true);

    useEffect(() => {

        let cancelled = false;

        (async () => {

            try {

                const [dashboardResult, appointmentsResult] = await Promise.all([

                    doctorDashboardService.getDashboard(),
                    doctorDashboardService.getTodayAppointments(),

                ]);

                const dashboardData = normalizeResponse(dashboardResult);

                if (!dashboardData.ok) {

                    throw new Error(
                        dashboardData.message || "Unable to load dashboard."
                    );

                }

                const appointmentsData = normalizeResponse(appointmentsResult);

                if (!appointmentsData.ok) {

                    throw new Error(
                        appointmentsData.message ||
                            "Unable to load today's appointments."
                    );

                }

                if (cancelled) return;

                setDashboard(dashboardData.data);

                setAppointments(
                    Array.isArray(appointmentsData.data)
                        ? appointmentsData.data
                        : []
                );

            }
            catch (error) {

                if (cancelled) return;

                console.error(error);

                const message =
                    error?.response?.data?.message ||
                    error?.message ||
                    "Something went wrong. Please try again.";

                toast.error(message);

            }
            finally {

                if (!cancelled) {
                    setLoading(false);
                }

            }

        })();

        return () => {
            cancelled = true;
        };

    }, []);

    if (loading) {

        return (
            <div className="dashboard-loading">
                Loading dashboard...
            </div>
        );

    }

    const todayLabel = new Date().toLocaleDateString(
        undefined,
        {
            weekday: "long",
            year: "numeric",
            month: "long",
            day: "numeric",
        }
    );

    return (

        <>
            <PageHeader
                title="Dashboard"
                subtitle={
                    `Welcome back, ${
                        dashboard?.DoctorName || user?.fullName || "Doctor"
                    }! Here's what's happening today.`
                }
            />

            <div className="dashboard-grid">

                <StatCard
                    title="Today's Appointments"
                    value={dashboard?.TotalAppointments ?? 0}
                    subtitle="Total for Today"
                    icon={<CalendarDays size={24} />}
                />

                <StatCard
                    title="Waiting"
                    value={dashboard?.WaitingAppointments ?? 0}
                    subtitle="Currently Waiting"
                    color="#F59E0B"
                    icon={<Clock size={24} />}
                />

                <StatCard
                    title="Completed"
                    value={dashboard?.CompletedAppointments ?? 0}
                    subtitle="Completed Today"
                    color="#15B79E"
                    icon={<CheckCircle2 size={24} />}
                />

                <StatCard
                    title="Upcoming"
                    value={dashboard?.UpcomingAppointments ?? 0}
                    subtitle="Upcoming Appointments"
                    color="#8B5CF6"
                    icon={<CalendarClock size={24} />}
                />

            </div>

            <div className="dashboard-bottom">

                <DashboardSection title={`Today's Appointments — ${todayLabel}`}>

                    {
                        appointments.length === 0 ? (

                            <div className="dashboard-empty">
                                <CalendarX2 size={40} />
                                <h4>No appointments today</h4>
                                <p>
                                    You&apos;re all caught up. New appointments will
                                    appear here.
                                </p>
                            </div>

                        ) : (

                            <div className="table-responsive appointment-table">

                                <table className="table table-hover align-middle mb-0">

                                    <thead>
                                        <tr>
                                            <th>Token</th>
                                            <th>Patient Name</th>
                                            <th>Appointment No.</th>
                                            <th>Time</th>
                                            <th>Consultation Type</th>
                                            <th>Status</th>
                                        </tr>
                                    </thead>

                                    <tbody>

                                        {
                                            appointments.map((appointment) => (

                                                <tr key={appointment.Id}>

                                                    <td className="token-cell">
                                                        {
                                                            appointment.TokenNumber ??
                                                                "—"
                                                        }
                                                    </td>

                                                    <td className="patient-cell">
                                                        {appointment.PatientName}
                                                    </td>

                                                    <td>
                                                        {appointment.AppointmentNumber}
                                                    </td>

                                                    <td>
                                                        {
                                                            formatTime(
                                                                appointment.AppointmentTime
                                                            )
                                                        }
                                                    </td>

                                                    <td>
                                                        {appointment.ConsultationType}
                                                    </td>

                                                    <td>
                                                        <StatusBadge
                                                            status={
                                                                appointment.AppointmentStatusName
                                                            }
                                                        />
                                                    </td>

                                                </tr>

                                            ))
                                        }

                                    </tbody>

                                </table>

                            </div>

                        )
                    }

                </DashboardSection>

            </div>

        </>

    );

}