"use client";

import { useEffect, useState } from "react";

import {
    CalendarDays,
    Building2,
    Stethoscope,
    Layers,
    CalendarX2,
    Info,
} from "lucide-react";

import { toast } from "react-toastify";

import hospitalAdminService from "@/services/hospital-admin/hospitalAdminService";
import DashboardSection from "@/components/super-admin/DashboardSection";
import StatCard from "@/components/super-admin/StatCard";
import PageHeader from "@/components/super-admin/PageHeader";
import StatusBadge from "@/components/portal/StatusBadge";
import { useAuth } from "@/contexts/AuthContext";

import "./Dashboard.css";

const normalizeResponse = (data) => {

    // Backend may return the raw PagedResponse, or the
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

const toLocalDateString = (date) => {

    const year = date.getFullYear();
    const month = String(date.getMonth() + 1).padStart(2, "0");
    const day = String(date.getDate()).padStart(2, "0");

    return `${year}-${month}-${day}`;

};

const MONTHS = [
    "Jan", "Feb", "Mar", "Apr", "May", "Jun",
    "Jul", "Aug", "Sep", "Oct", "Nov", "Dec",
];

const formatDate = (value) => {

    if (typeof value !== "string" || !value.includes("-")) {
        return value || "—";
    }

    const [year, month, day] = value.split("-");

    return `${day} ${MONTHS[Number(month) - 1] || month} ${year}`;

};

const formatTime = (value) =>
    typeof value === "string" && value.includes(":")
        ? value.slice(0, 5)
        : value || "—";

const fetchCount = async (loader, params) => {

    try {

        const response = await loader(params);

        const normalized = normalizeResponse(response);

        if (!normalized.ok) {
            return null;
        }

        const paged = normalized.data;

        return typeof paged?.totalCount === "number"
            ? paged.totalCount
            : null;

    }
    catch (error) {

        console.error(error);

        return null;

    }

};

export default function HospitalAdminDashboardPage() {

    const { user } = useAuth();

    const [counts, setCounts] = useState({
        branches: null,
        departments: null,
        doctors: null,
    });

    const [appointments, setAppointments] = useState([]);

    const [totalCount, setTotalCount] = useState(0);

    const [totalPages, setTotalPages] = useState(1);

    const [loading, setLoading] = useState(true);

    useEffect(() => {

        let cancelled = false;

        (async () => {

            try {

                const today = toLocalDateString(new Date());

                const [branches, departments, doctors, todayResponse] =
                    await Promise.all([
                        fetchCount(
                            hospitalAdminService.getBranches,
                            { page: 1, pageSize: 1 }
                        ),
                        fetchCount(
                            hospitalAdminService.getDepartments,
                            { page: 1, pageSize: 1 }
                        ),
                        fetchCount(
                            hospitalAdminService.getDoctors,
                            { page: 1, pageSize: 1 }
                        ),
                        hospitalAdminService.getToday({
                            fromDate: today,
                            toDate: today,
                            page: 1,
                            pageSize: 100,
                        }),
                    ]);

                if (cancelled) return;

                const nextCounts = { branches, departments, doctors };

                setCounts(nextCounts);

                const unavailable = Object.values(nextCounts)
                    .filter((value) => value === null).length;

                if (unavailable > 0) {
                    toast.warn(
                        "Some statistics could not be loaded and are shown as —."
                    );
                }

                const normalized = normalizeResponse(todayResponse);

                if (!normalized.ok) {

                    throw new Error(
                        normalized.message ||
                            "Unable to load today's appointments."
                    );

                }

                const paged = normalized.data;

                if (!paged || typeof paged !== "object") {

                    throw new Error(
                        "No data received from the server."
                    );

                }

                const items = Array.isArray(paged.items) ? paged.items : [];

                setAppointments(items);

                setTotalCount(
                    typeof paged.totalCount === "number"
                        ? paged.totalCount
                        : items.length
                );

                setTotalPages(
                    typeof paged.totalPages === "number"
                        ? paged.totalPages
                        : 1
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

                setAppointments([]);

                setTotalCount(0);

                setTotalPages(1);

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

    const checkedInCount = appointments.filter(
        (appointment) =>
            String(appointment.appointmentStatusName || "")
                .toUpperCase()
                .replace(/[^A-Z0-9]+/g, "_") === "CHECKED_IN"
    ).length;

    const inQueueCount = appointments.filter((appointment) => {

        const code = String(appointment.appointmentStatusName || "")
            .toUpperCase()
            .replace(/[^A-Z0-9]+/g, "_");

        return (
            code === "WAITING" ||
            code === "CHECKED_IN" ||
            code === "CALLED"
        );

    }).length;

    const completedCount = appointments.filter(
        (appointment) =>
            String(appointment.appointmentStatusName || "")
                .toUpperCase()
                .replace(/[^A-Z0-9]+/g, "_") === "COMPLETED"
    ).length;

    const isPartial = totalPages > 1;

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
                subtitle={`Welcome back, ${
                    user?.fullName || "Administrator"
                }! Here is what is happening in your hospital today.`}
            />

            <div className="dashboard-grid">

                <StatCard
                    title="Today's Appointments"
                    value={totalCount}
                    subtitle="Total for Today"
                    icon={<CalendarDays size={24} />}
                />

                <StatCard
                    title="Doctors"
                    value={counts.doctors ?? "—"}
                    subtitle="All doctors"
                    color="#8B5CF6"
                    icon={<Stethoscope size={24} />}
                />

                <StatCard
                    title="Branches"
                    value={counts.branches ?? "—"}
                    subtitle="All branches"
                    color="#15B79E"
                    icon={<Building2 size={24} />}
                />

                <StatCard
                    title="Departments"
                    value={counts.departments ?? "—"}
                    subtitle="All departments"
                    color="#F59E0B"
                    icon={<Layers size={24} />}
                />

            </div>

            <div className="dashboard-note">

                <Info size={16} />

                <span>
                    Counts reflect live hospital records. Management screens
                    for Branches, Departments, Doctors, Staff and Roles are
                    coming soon.
                </span>

            </div>

            {
                isPartial && (

                    <div className="dashboard-note">

                        <Info size={16} />

                        <span>
                            Status summaries are calculated from the loaded
                            first page (page 1 of {totalPages}, up to 100
                            appointments). The Total Appointments card shows
                            the server count for today.
                        </span>

                    </div>

                )
            }

            <div className="dashboard-summary">

                <StatCard
                    title="Checked In"
                    value={checkedInCount}
                    subtitle={isPartial ? "First page only" : "Today"}
                    color="#15B79E"
                />

                <StatCard
                    title="In Queue"
                    value={inQueueCount}
                    subtitle={
                        isPartial
                            ? "First page only"
                            : "Waiting, Checked In & Called"
                    }
                    color="#F59E0B"
                />

                <StatCard
                    title="Completed"
                    value={completedCount}
                    subtitle={isPartial ? "First page only" : "Today"}
                    color="#8B5CF6"
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
                                    Today&apos;s appointments will appear here as
                                    they are booked or walked in.
                                </p>
                            </div>

                        ) : (

                            <div className="table-responsive appointment-table">

                                <table className="table table-hover align-middle mb-0">

                                    <thead>
                                        <tr>
                                            <th>Appointment No.</th>
                                            <th>Date</th>
                                            <th>Time</th>
                                            <th>Consultation Type</th>
                                            <th>Status</th>
                                            <th>Token</th>
                                        </tr>
                                    </thead>

                                    <tbody>

                                        {
                                            appointments.map((appointment) => (

                                                <tr key={appointment.id}>

                                                    <td className="appointment-number-cell">
                                                        {appointment.appointmentNumber}
                                                    </td>

                                                    <td>
                                                        {formatDate(appointment.appointmentDate)}
                                                    </td>

                                                    <td>
                                                        {formatTime(appointment.appointmentTime)}
                                                    </td>

                                                    <td>
                                                        {appointment.consultationType || "—"}
                                                    </td>

                                                    <td>
                                                        <StatusBadge
                                                            status={
                                                                appointment.appointmentStatusName
                                                            }
                                                        />
                                                    </td>

                                                    <td className="token-cell">
                                                        {appointment.tokenNumber ?? "—"}
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