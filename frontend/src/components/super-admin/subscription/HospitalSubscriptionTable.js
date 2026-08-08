"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

import {
    Eye,
    Pencil,
    Plus,
    RefreshCw,
    XCircle,
    Trash2,
} from "lucide-react";

import { toast } from "react-toastify";
import Swal from "sweetalert2";

import hospitalSubscriptionService
    from "@/services/subscription/hospitalSubscriptionService";

import "./HospitalSubscriptionTable.css";

export default function HospitalSubscriptionTable() {

    const [subscriptions, setSubscriptions] = useState([]);

    const [loading, setLoading] = useState(true);


    useEffect(() => {

        loadSubscriptions();

    }, []);
    const handleRenew = async (subscription) => {

        const result = await Swal.fire({
            title: "Renew Subscription?",
            text: `Renew ${subscription.planName} subscription for ${subscription.hospitalName}?`,
            icon: "question",
            showCancelButton: true,
            confirmButtonText: "Yes, Renew",
            cancelButtonText: "Cancel",
        });

        if (!result.isConfirmed) return;

        try {

            const response =
                await hospitalSubscriptionService.renewSubscription(
                    subscription.id
                );

            if (response.success) {

                await Swal.fire({
                    title: "Renewed!",
                    text: response.message,
                    icon: "success",
                    confirmButtonText: "OK",
                });

                loadSubscriptions();

            }
            else {

                Swal.fire({
                    title: "Failed",
                    text: response.message,
                    icon: "error",
                });

            }

        }
        catch (error) {

            console.error(error);

            Swal.fire({
                title: "Error",
                text: "Something went wrong.",
                icon: "error",
            });

        }

    };

    const handleCancel = async (subscription) => {

        const result = await Swal.fire({
            title: "Cancel Subscription?",
            text: `Are you sure you want to cancel the subscription for ${subscription.hospitalName}?`,
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, Cancel",
            cancelButtonText: "Keep Active",
        });

        if (!result.isConfirmed) return;

        try {

            const response =
                await hospitalSubscriptionService.cancelSubscription(
                    subscription.id
                );

            if (response.success) {

                await Swal.fire({
                    title: "Cancelled!",
                    text: response.message,
                    icon: "success",
                    confirmButtonText: "OK",
                });

                loadSubscriptions();

            }
            else {

                Swal.fire({
                    title: "Failed",
                    text: response.message,
                    icon: "error",
                });

            }

        }
        catch (error) {

            console.error(error);

            Swal.fire({
                title: "Error",
                text: "Something went wrong.",
                icon: "error",
            });

        }

    };

    const handleDelete = async (subscription) => {

        const result = await Swal.fire({
            title: "Delete Subscription?",
            text: `This will permanently delete the subscription for ${subscription.hospitalName}.`,
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Yes, Delete",
            cancelButtonText: "Cancel",
            confirmButtonColor: "#dc3545",
        });

        if (!result.isConfirmed) return;

        try {

            const response =
                await hospitalSubscriptionService.deleteSubscription(
                    subscription.id
                );

            if (response.success) {

                await Swal.fire({
                    title: "Deleted!",
                    text: response.message,
                    icon: "success",
                    confirmButtonText: "OK",
                });

                loadSubscriptions();

            }
            else {

                Swal.fire({
                    title: "Failed",
                    text: response.message,
                    icon: "error",
                });

            }

        }
        catch (error) {

            console.error(error);

            Swal.fire({
                title: "Error",
                text: "Something went wrong.",
                icon: "error",
            });

        }

    };


    const loadSubscriptions = async () => {

        try {

            setLoading(true);

            const response =
                await hospitalSubscriptionService.getSubscriptions();

            if (response.success) {

                setSubscriptions(response.data);

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error(
                "Failed to load subscriptions."
            );

        }
        finally {

            setLoading(false);

        }

    };


    if (loading) {

        return (

            <div className="table-card">

                <div className="table-loading">

                    Loading subscriptions...

                </div>

            </div>

        );

    }


    return (

        <div className="table-card">

            {/* Header */}

            <div className="table-header">

                <h3>

                    Hospital Subscriptions

                </h3>

                <Link
                    href="/super-admin/subscriptions/create"
                    className="btn btn-primary"
                >

                    <Plus size={18} />

                    Add Subscription

                </Link>

            </div>


            {/* Empty */}

            {subscriptions.length === 0 ? (

                <div className="empty-state">

                    No subscriptions found.

                </div>

            ) : (

                <div className="table-responsive">

                    <table className="table hospital-subscription-table">

                        <thead>

                            <tr>

                                <th>Hospital</th>

                                <th>Plan</th>

                                <th>Start Date</th>

                                <th>End Date</th>

                                <th>Amount</th>

                                <th>Payment</th>

                                <th>Status</th>

                                <th>Action</th>

                            </tr>

                        </thead>


                        <tbody>

                            {subscriptions.map(
                                (subscription) => (

                                    <tr
                                        key={
                                            subscription.id
                                        }
                                    >

                                        <td>

                                            <strong>

                                                {
                                                    subscription.hospitalName
                                                }

                                            </strong>

                                        </td>


                                        <td>

                                            {
                                                subscription.planName
                                            }

                                        </td>


                                        <td>

                                            {
                                                new Date(
                                                    subscription.startDate
                                                ).toLocaleDateString()
                                            }

                                        </td>


                                        <td>

                                            {
                                                new Date(
                                                    subscription.endDate
                                                ).toLocaleDateString()
                                            }

                                        </td>


                                        <td>

                                            ₹
                                            {
                                                subscription.amountPaid
                                            }

                                        </td>


                                        <td>

                                            <span
                                                className={
                                                    subscription.paymentStatus ===
                                                        "Paid"
                                                        ? "badge-active"
                                                        : "badge-inactive"
                                                }
                                            >

                                                {
                                                    subscription.paymentStatus
                                                }

                                            </span>

                                        </td>


                                        <td>

                                            <span
                                                className={
                                                    subscription.isActive
                                                        ? "badge-active"
                                                        : "badge-inactive"
                                                }
                                            >

                                                {
                                                    subscription.isActive
                                                        ? "Active"
                                                        : "Inactive"
                                                }

                                            </span>

                                        </td>


                                        <td>

                                            <div className="action-buttons">

                                                {/* View */}

                                                <Link
                                                    href={`/super-admin/subscriptions/view/${subscription.id}`}
                                                    className="action-btn view"
                                                    title="View"
                                                >

                                                    <Eye
                                                        size={16}
                                                    />

                                                </Link>


                                                {/* Edit */}

                                                <Link
                                                    href={`/super-admin/subscriptions/edit/${subscription.id}`}
                                                    className="action-btn edit"
                                                    title="Edit"
                                                >

                                                    <Pencil
                                                        size={16}
                                                    />

                                                </Link>


                                                {/* Renew */}

                                                <button
                                                    className="action-btn renew"
                                                    title="Renew"
                                                    onClick={() => handleRenew(subscription)}
                                                >
                                                    <RefreshCw size={16} />
                                                </button>


                                                {/* Cancel */}

                                                <button
                                                    className="action-btn cancel"
                                                    title="Cancel"
                                                    onClick={() => handleCancel(subscription)}
                                                >
                                                    <XCircle size={16} />
                                                </button>


                                                {/* Delete */}

                                               <button
    className="action-btn delete"
    title="Delete"
    onClick={() => handleDelete(subscription)}
>
    <Trash2 size={16} />
</button>

                                            </div>

                                        </td>

                                    </tr>

                                )
                            )}

                        </tbody>

                    </table>

                </div>

            )}

        </div>

    );

}