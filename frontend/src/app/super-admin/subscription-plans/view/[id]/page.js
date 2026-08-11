"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { toast } from "react-toastify";

import subscriptionPlanService
    from "@/services/subscription/subscriptionPlanService";

export default function ViewSubscriptionPlanPage() {

    const { id } = useParams();
    const router = useRouter();

    const [plan, setPlan] = useState(null);
    const [loading, setLoading] = useState(true);

    useEffect(() => {

        if (id) {
            loadPlan();
        }

    }, [id]);

    const loadPlan = async () => {

        try {

            const response =
                await subscriptionPlanService.getSubscriptionPlanById(id);

            console.log("Plan Response:", response);

            if (response.success) {

                setPlan(response.data);

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Failed to load subscription plan.");

        }
        finally {

            setLoading(false);

        }

    };

    if (loading) {

        return <p>Loading...</p>;

    }

    if (!plan) {

        return (
            <div className="alert alert-danger">
                Subscription plan not found.
            </div>
        );

    }

    return (

        <div className="form-card">

            <div className="d-flex justify-content-between align-items-center mb-4">

                <h4>
                    Subscription Plan Details
                </h4>

                <button
                    className="btn btn-light"
                    onClick={() =>
                        router.push("/super-admin/subscription-plans")
                    }
                >
                    Back
                </button>

            </div>

            <div className="row">

                <div className="col-md-6 mb-3">

                    <label className="form-label">
                        Plan Name
                    </label>

                    <div className="form-control bg-light">
                        {plan.name}
                    </div>

                </div>

                <div className="col-md-6 mb-3">

                    <label className="form-label">
                        Price
                    </label>

                    <div className="form-control bg-light">
                        ₹{plan.price}
                    </div>

                </div>

                <div className="col-md-6 mb-3">

                    <label className="form-label">
                        Duration
                    </label>

                    <div className="form-control bg-light">
                        {plan.durationInDays} Days
                    </div>

                </div>

                <div className="col-md-6 mb-3">

                    <label className="form-label">
                        Trial Plan
                    </label>

                    <div className="form-control bg-light">
                        {plan.isTrial ? "Yes" : "No"}
                    </div>

                </div>

                <div className="col-md-6 mb-3">

                    <label className="form-label">
                        Maximum Branches
                    </label>

                    <div className="form-control bg-light">
                        {plan.maxBranches}
                    </div>

                </div>

                <div className="col-md-6 mb-3">

                    <label className="form-label">
                        Maximum Doctors
                    </label>

                    <div className="form-control bg-light">
                        {plan.maxDoctors}
                    </div>

                </div>

                <div className="col-md-6 mb-3">

                    <label className="form-label">
                        Maximum Patients
                    </label>

                    <div className="form-control bg-light">
                        {plan.maxPatients}
                    </div>

                </div>

                <div className="col-md-6 mb-3">

                    <label className="form-label">
                        Status
                    </label>

                    <div className="form-control bg-light">

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

                    </div>

                </div>

                <div className="col-md-12 mb-3">

                    <label className="form-label">
                        Description
                    </label>

                    <div className="form-control bg-light">
                        {plan.description || "No description"}
                    </div>

                </div>

            </div>

        </div>

    );

}