"use client";

import PageHeader from "@/components/super-admin/PageHeader";
import HospitalTable from "@/components/super-admin/hospital/HospitalTable";

import "./Hospital.css";

export default function HospitalPage() {

    return (

        <>

            <PageHeader
                title="Hospitals"
                subtitle="Manage all registered hospitals."
            />

            <HospitalTable />

        </>

    );

}