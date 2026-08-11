"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

import {
    Eye,
    Pencil,
    Plus,
    Trash2,
} from "lucide-react";

import { toast } from "react-toastify";
import Swal from "sweetalert2";

import subscriptionPlanService
    from "@/services/subscription/subscriptionPlanService";

import "./SubscriptionPlanTable.css";

export default function SubscriptionPlanTable() {

    const [plans, setPlans] = useState([]);

    const [loading, setLoading] = useState(true);


    useEffect(() => {

        loadPlans();

    }, []);


    const loadPlans = async () => {

        try {

            setLoading(true);

            const response =
                await subscriptionPlanService.getSubscriptionPlans();

            if (response.success) {

                setPlans(response.data || []);

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error(
                "Failed to load subscription plans."
            );

        }
        finally {

            setLoading(false);

        }

    };


    const handleDelete = async (plan) => {

        const result = await Swal.fire({

            title: "Delete Subscription Plan?",

            text: `Are you sure you want to delete "${plan.name}"?`,

            icon: "warning",

            showCancelButton: true,

            confirmButtonText: "Yes, Delete",

            cancelButtonText: "Cancel",

        });


        if (!result.isConfirmed) {

            return;

        }


        try {

            const response =
                await subscriptionPlanService.deleteSubscriptionPlan(
                    plan.id
                );


            if (response.success) {

                toast.success(response.message);

                loadPlans();

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error(
                "Failed to delete subscription plan."
            );

        }

    };


    if (loading) {

        return (

            <div className="table-card">

                <div className="table-loading">

                    Loading subscription plans...

                </div>

            </div>

        );

    }


    return (

        <div className="table-card">

            {/* Header */}

            <div className="table-header">

                <h3>

                    Subscription Plans

                </h3>


                <Link
                    href="/super-admin/subscription-plans/create"
                    className="btn btn-primary"
                >

                    <Plus size={18} />

                    Add Plan

                </Link>

            </div>


            {/* Empty State */}

            {plans.length === 0 ? (

                <div className="empty-state">

                    No subscription plans found.

                </div>

            ) : (

                <div className="table-responsive">

                    <table className="table subscription-plan-table">

                        <thead>

                            <tr>

                                <th>Plan</th>

                                <th>Price</th>

                                <th>Duration</th>

                                <th>Type</th>

                                <th>Status</th>

                                <th width="130">
                                    Action
                                </th>

                            </tr>

                        </thead>


                        <tbody>

                            {plans.map((plan) => (

                                <tr
                                    key={plan.id}
                                >

                                    <td>

                                        <strong>

                                            {plan.name}

                                        </strong>

                                    </td>


                                    <td>

                                        ₹{plan.price}

                                    </td>


                                    <td>

                                        {plan.durationInDays} days

                                    </td>


                                    <td>

                                        <span
                                            className={
                                                plan.isTrial
                                                    ? "badge-trial"
                                                    : "badge-paid"
                                            }
                                        >

                                            {plan.isTrial
                                                ? "Trial"
                                                : "Paid"}

                                        </span>

                                    </td>


                                    <td>

                                        <span
                                            className={
                                                plan.isActive
                                                    ? "badge-active"
                                                    : "badge-inactive"
                                            }
                                        >

                                            {plan.isActive
                                                ? "Active"
                                                : "Inactive"}

                                        </span>

                                    </td>


                                    <td>

                                        <div className="action-buttons">

                                            {/* View */}

                                            <Link
                                                href={`/super-admin/subscription-plans/view/${plan.id}`}
                                                className="action-btn view"
                                                title="View"
                                            >

                                                <Eye size={16} />

                                            </Link>


                                            {/* Edit */}

                                            <Link
                                                href={`/super-admin/subscription-plans/edit/${plan.id}`}
                                                className="action-btn edit"
                                                title="Edit"
                                            >

                                                <Pencil size={16} />

                                            </Link>


                                            {/* Delete */}

                                            <button
                                                className="action-btn delete"
                                                title="Delete"
                                                onClick={() =>
                                                    handleDelete(plan)
                                                }
                                            >

                                                <Trash2 size={16} />

                                            </button>

                                        </div>

                                    </td>

                                </tr>

                            ))}

                        </tbody>

                    </table>

                </div>

            )}

        </div>

    );

}