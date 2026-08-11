"use client";

import { useRouter } from "next/navigation";
import { toast } from "react-toastify";
import { useEffect, useState } from "react";
import subscriptionPlanService
    from "@/services/subscription/subscriptionPlanService";

import "./SubscriptionPlanForm.css";

export default function SubscriptionPlanForm({
    initialData = null,
    isEdit = false,
}) {
    const router = useRouter();

    const [loading, setLoading] = useState(false);

    const [formData, setFormData] = useState({

        name: initialData?.name || "",
        description: initialData?.description || "",
        price: initialData?.price ?? "",
        durationInDays: initialData?.durationInDays ?? "",
        maxBranches: initialData?.maxBranches ?? "",
        maxDoctors: initialData?.maxDoctors ?? "",
        maxPatients: initialData?.maxPatients ?? "",
        isTrial: initialData?.isTrial || false,
        isActive: initialData?.isActive ?? true,

    });

    useEffect(() => {

        if (!initialData) return;

        setFormData({

            name: initialData.name || "",

            description: initialData.description || "",

            price: initialData.price ?? "",

            durationInDays:
                initialData.durationInDays ?? "",

            maxBranches:
                initialData.maxBranches ?? "",

            maxDoctors:
                initialData.maxDoctors ?? "",

            maxPatients:
                initialData.maxPatients ?? "",

            isTrial:
                initialData.isTrial || false,

            isActive:
                initialData.isActive ?? true,

        });

    }, [initialData]);

    const handleChange = (e) => {

        const { name, value, type, checked } = e.target;

        setFormData((prev) => ({

            ...prev,

            [name]:
                type === "checkbox"
                    ? checked
                    : value,

        }));

    };

    const handleSubmit = async (e) => {

        e.preventDefault();

        if (!formData.name.trim()) {

            toast.error("Plan name is required.");

            return;

        }

        if (!formData.price && formData.price !== 0) {

            toast.error("Price is required.");

            return;

        }

        if (!formData.durationInDays) {

            toast.error("Duration is required.");

            return;

        }

        try {

            setLoading(true);

            const request = {

                name: formData.name.trim(),

                description:
                    formData.description.trim() || null,

                price: Number(formData.price),

                durationInDays:
                    Number(formData.durationInDays),

                maxBranches:
                    Number(formData.maxBranches),

                maxDoctors:
                    Number(formData.maxDoctors),

                maxPatients:
                    Number(formData.maxPatients),

                isTrial: formData.isTrial,

                isActive: formData.isActive,

            };

            let response;

            if (isEdit) {

                response =
                    await subscriptionPlanService
                        .updateSubscriptionPlan(
                            initialData.id,
                            request
                        );

            }
            else {

                response =
                    await subscriptionPlanService
                        .createSubscriptionPlan(request);

            }
            if (response.success) {

                toast.success(
                    isEdit
                        ? "Subscription plan updated successfully."
                        : "Subscription plan created successfully."
                );

                router.push(
                    "/super-admin/subscription-plans"
                );

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(
                "Subscription Plan Error:",
                error
            );

            const message =
                error.response?.data?.message ||
                error.response?.data?.errors?.message?.[0] ||
                "Something went wrong.";

            toast.error(message);

        }
        finally {

            setLoading(false);

        }

    };

    return (

        <div className="form-card">

            <form onSubmit={handleSubmit}>

                <div className="form-section">

                    <h5>
                        Subscription Plan Information
                    </h5>

                    <div className="row">

                        {/* Plan Name */}

                        <div className="col-md-6 mb-3">

                            <label className="form-label">
                                Plan Name
                            </label>

                            <input
                                autoFocus
                                type="text"
                                className="form-control"
                                name="name"
                                value={formData.name}
                                onChange={handleChange}
                                placeholder="Enter plan name"
                            />

                        </div>

                        {/* Price */}

                        <div className="col-md-6 mb-3">

                            <label className="form-label">
                                Price
                            </label>

                            <input
                                type="number"
                                className="form-control"
                                name="price"
                                value={formData.price}
                                onChange={handleChange}
                                min="0"
                                placeholder="Enter price"
                            />

                        </div>

                        {/* Duration */}

                        <div className="col-md-6 mb-3">

                            <label className="form-label">
                                Duration (Days)
                            </label>

                            <input
                                type="number"
                                className="form-control"
                                name="durationInDays"
                                value={formData.durationInDays}
                                onChange={handleChange}
                                min="1"
                                placeholder="Example: 365"
                            />

                        </div>

                        {/* Description */}

                        <div className="col-md-6 mb-3">

                            <label className="form-label">
                                Description
                            </label>

                            <input
                                type="text"
                                className="form-control"
                                name="description"
                                value={formData.description}
                                onChange={handleChange}
                                placeholder="Enter plan description"
                            />

                        </div>

                    </div>

                </div>


                {/* Limits */}

                <div className="form-section">

                    <h5>
                        Plan Limits
                    </h5>

                    <div className="row">

                        {/* Branches */}

                        <div className="col-md-4 mb-3">

                            <label className="form-label">
                                Max Branches
                            </label>

                            <input
                                type="number"
                                className="form-control"
                                name="maxBranches"
                                value={formData.maxBranches}
                                onChange={handleChange}
                                min="0"
                            />

                        </div>

                        {/* Doctors */}

                        <div className="col-md-4 mb-3">

                            <label className="form-label">
                                Max Doctors
                            </label>

                            <input
                                type="number"
                                className="form-control"
                                name="maxDoctors"
                                value={formData.maxDoctors}
                                onChange={handleChange}
                                min="0"
                            />

                        </div>

                        {/* Patients */}

                        <div className="col-md-4 mb-3">

                            <label className="form-label">
                                Max Patients
                            </label>

                            <input
                                type="number"
                                className="form-control"
                                name="maxPatients"
                                value={formData.maxPatients}
                                onChange={handleChange}
                                min="0"
                            />

                        </div>

                    </div>

                </div>


                {/* Settings */}

                <div className="form-section">

                    <h5>
                        Plan Settings
                    </h5>

                    <div className="row">

                        <div className="col-md-6 mb-3">

                            <div className="form-check">

                                <input
                                    type="checkbox"
                                    className="form-check-input"
                                    id="isTrial"
                                    name="isTrial"
                                    checked={formData.isTrial}
                                    onChange={handleChange}
                                />

                                <label
                                    className="form-check-label"
                                    htmlFor="isTrial"
                                >
                                    Trial Plan
                                </label>

                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <div className="form-check">

                                <input
                                    type="checkbox"
                                    className="form-check-input"
                                    id="isActive"
                                    name="isActive"
                                    checked={formData.isActive}
                                    onChange={handleChange}
                                />

                                <label
                                    className="form-check-label"
                                    htmlFor="isActive"
                                >
                                    Active
                                </label>

                            </div>

                        </div>

                    </div>

                </div>


                {/* Actions */}

                <div className="form-actions">

                    <button
                        type="button"
                        className="btn btn-light"
                        onClick={() =>
                            router.push(
                                "/super-admin/subscription-plans"
                            )
                        }
                    >
                        Cancel
                    </button>
                    <button
                        type="submit"
                        className="btn btn-primary"
                        disabled={loading}
                    >

                        {loading
                            ? (isEdit ? "Updating..." : "Saving...")
                            : (isEdit ? "Update Plan" : "Save Plan")}

                    </button>


                </div>

            </form>

        </div>

    );

}