"use client";

import { useEffect, useState } from "react";
import Link from "next/link";
import { toast } from "react-toastify";
import Swal from "sweetalert2";
import {
    Eye,
    Pencil,
    Trash2,
    Plus,
    Power,
} from "lucide-react";
import hospitalService from "@/services/hospital/hospitalService";
import HospitalFilter from "./HospitalFilter";
import Pagination from "@/components/common/Pagination";

import "./HospitalTable.css";

export default function HospitalTable() {

    const [loading, setLoading] = useState(true);

    const [hospitals, setHospitals] = useState([]);

    const [page, setPage] = useState(1);

    const [pageSize] = useState(10);

    const [totalPages, setTotalPages] = useState(1);

    const [totalRecords, setTotalRecords] = useState(0);
    const [search, setSearch] = useState("");

    const [status, setStatus] = useState("");

    useEffect(() => {

        loadHospitals();

    }, [page, search, status]);

    const handleActivate = async (hospitalId) => {

        const confirmed = window.confirm(
            "Are you sure you want to activate this hospital?"
        );

        if (!confirmed) return;

        try {

            const response =
                await hospitalService.activateHospital(hospitalId);

            if (response.success) {

                toast.success(response.message);

                await loadHospitals();

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };
    const handleDeactivate = async (hospitalId) => {

        const confirmed = window.confirm(
            "Are you sure you want to deactivate this hospital?"
        );

        if (!confirmed) return;

        try {

            const response =
                await hospitalService.deactivateHospital(hospitalId);

            if (response.success) {

                toast.success(response.message);

                await loadHospitals();

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };
    const loadHospitals = async () => {

        try {

            const response = await hospitalService.getHospitals({

                page,

                pageSize,

                search,

                status,

            });

            if (response.success) {

                setHospitals(response.data.items);

                setTotalPages(response.data.totalPages);

                setTotalRecords(response.data.totalRecords);

            }

        }
        catch (error) {

            console.log(error);

        }
        finally {

            setLoading(false);

        }

    };
    const handleStatusChange = async (hospital) => {

        const isActive = hospital.isActive;

        const result = await Swal.fire({

            title: isActive
                ? "Deactivate Hospital?"
                : "Activate Hospital?",

            text: isActive
                ? "The hospital will become inactive."
                : "The hospital will become active.",

            icon: "warning",

            showCancelButton: true,

            confirmButtonText: isActive
                ? "Yes, Deactivate"
                : "Yes, Activate",

            cancelButtonText: "Cancel",

            reverseButtons: true,

        });

        if (!result.isConfirmed) return;

        try {

            const response = isActive

                ? await hospitalService.deactivateHospital(
                    hospital.hospitalId
                )

                : await hospitalService.activateHospital(
                    hospital.hospitalId
                );

            if (response.success) {

                toast.success(response.message);

                loadHospitals();

            } else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };
    const handleDelete = async (hospitalId) => {

        const result = await Swal.fire({
            title: "Delete Hospital?",
            text: "This hospital will be deleted.",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, Delete",
            cancelButtonText: "Cancel",
            reverseButtons: true,
        });

        if (!result.isConfirmed) return;

        try {

            const response =
                await hospitalService.deleteHospital(hospitalId);

            if (response.success) {

                toast.success(response.message);

                loadHospitals();

            } else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };


    if (loading) {

        return (

            <div className="table-card">

                <div className="table-loading">

                    Loading hospitals...

                </div>

            </div>

        );

    }

    return (

        <div className="table-card">

            <div className="table-header">

                <h3>

                    Hospital List

                </h3>

                <Link
                    href="/super-admin/hospitals/create"
                    className="btn btn-primary"
                >

                    <Plus size={18} />

                    Add Hospital

                </Link>

            </div>
            <HospitalFilter

                search={search}
                setSearch={setSearch}

                status={status}
                setStatus={setStatus}

            />

            {

                hospitals.length === 0 ?

                    (

                        <div className="empty-state">

                            No hospitals found.

                        </div>

                    )

                    :

                    (

                        <div className="table-responsive">

                            <table className="table hospital-table">

                                <thead>

                                    <tr>

                                        <th>Code</th>

                                        <th>Hospital</th>

                                        <th>Email</th>

                                        <th>Phone</th>

                                        <th>Status</th>

                                        <th>Created</th>

                                        <th width="150">

                                            Action

                                        </th>

                                    </tr>

                                </thead>

                                <tbody>

                                    {

                                        hospitals.map((hospital) => (

                                            <tr
                                                key={hospital.hospitalId}
                                            >

                                                <td>

                                                    {hospital.code}

                                                </td>

                                                <td>

                                                    <strong>

                                                        {hospital.name}

                                                    </strong>

                                                </td>

                                                <td>

                                                    {hospital.email}

                                                </td>

                                                <td>

                                                    {hospital.phoneNumber}

                                                </td>

                                                <td>

                                                    <span
                                                        className={
                                                            hospital.isActive
                                                                ? "badge-active"
                                                                : "badge-inactive"
                                                        }
                                                    >

                                                        {

                                                            hospital.isActive
                                                                ? "Active"
                                                                : "Inactive"

                                                        }

                                                    </span>

                                                </td>

                                                <td>

                                                    {

                                                        new Date(
                                                            hospital.createdAt
                                                        ).toLocaleDateString()

                                                    }

                                                </td>

                                                <td>

                                                    <div className="action-buttons">

                                                        <Link
                                                            href={`/super-admin/hospitals/view/${hospital.hospitalId}`}
                                                            className="action-btn view"
                                                        >

                                                            <Eye size={16} />

                                                        </Link>

                                                        <Link
                                                            href={`/super-admin/hospitals/edit/${hospital.hospitalId}`}
                                                            className="action-btn edit"
                                                        >

                                                            <Pencil size={16} />

                                                        </Link>
                                                        <button
                                                            className={
                                                                hospital.isActive
                                                                    ? "action-btn deactivate"
                                                                    : "action-btn activate"
                                                            }
                                                            onClick={() => handleStatusChange(hospital)}
                                                            title={hospital.isActive ? "Deactivate" : "Activate"}
                                                        >
                                                            <Power size={16} />
                                                        </button>
                                                        <button
                                                            className="btn btn-sm btn-danger"
                                                            onClick={() => handleDelete(hospital.hospitalId)}
                                                        >

                                                            <Trash2 size={16} />

                                                        </button>

                                                    </div>

                                                </td>

                                            </tr>

                                        ))

                                    }

                                </tbody>

                            </table>
                            <Pagination

                                currentPage={page}

                                totalPages={totalPages}

                                onPageChange={setPage}

                            />
                        </div>

                    )

            }

        </div>

    );

}