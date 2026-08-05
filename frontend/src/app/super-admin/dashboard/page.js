"use client";

import { useEffect, useState } from "react";

import {
    Building2,
    CreditCard,
    IndianRupee,
} from "lucide-react";

import dashboardService from "@/services/dashboard/dashboardService";
import DashboardSection from "@/components/super-admin/DashboardSection";
import RecentHospitals from "@/components/super-admin/RecentHospitals";
import SubscriptionOverview from "@/components/super-admin/SubscriptionOverview";
import RevenueChart from "@/components/super-admin/RevenueChart";
import PageHeader from "@/components/super-admin/PageHeader";
import StatCard from "@/components/super-admin/StatCard";

import "./Dashboard.css";

export default function DashboardPage() {

const [dashboard, setDashboard] = useState(null);

const [loading, setLoading] = useState(true);

const loadDashboard = async () => {

    try {

        const response = await dashboardService.getDashboard();

        if (response.success) {

            setDashboard(response.data);

        }

    }
    catch (error) {

        console.log(error);

    }
    finally {

        setLoading(false);

    }

};

useEffect(() => {

    loadDashboard();

}, []);

    return (

        <>

            <PageHeader
                title="Dashboard"
                subtitle="Welcome back! Here's what's happening today."
            />

            <div className="dashboard-grid">

    <StatCard
        title="Hospitals"
        value={dashboard?.totalHospitals ?? 0}
        subtitle="Registered Hospitals"
        icon={<Building2 size={24} />}
    />

    <StatCard
        title="Active Hospitals"
        value={dashboard?.activeHospitals ?? 0}
        subtitle="Currently Active"
        color="#15B79E"
        icon={<Building2 size={24} />}
    />

    <StatCard
        title="Subscription Plans"
        value={dashboard?.totalSubscriptionPlans ?? 0}
        subtitle="Available Plans"
        color="#F59E0B"
        icon={<CreditCard size={24} />}
    />

    <StatCard
        title="Monthly Revenue"
        value={`₹${dashboard?.monthlyRevenue ?? 0}`}
        subtitle="Current Month"
        color="#EF4444"
        icon={<IndianRupee size={24} />}
    />

</div>

<div className="dashboard-content">

    <DashboardSection title="Recent Hospitals">

        <RecentHospitals />

    </DashboardSection>

    <DashboardSection title="Subscription Overview">

        <SubscriptionOverview dashboard={dashboard} />

    </DashboardSection>

</div>

<div className="dashboard-bottom">

    <DashboardSection title="Revenue Analytics">

    <RevenueChart/>

</DashboardSection>

</div>

        </>

    );

}