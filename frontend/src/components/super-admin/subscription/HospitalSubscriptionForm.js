"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { toast } from "react-toastify";

import hospitalService from "@/services/hospital/hospitalService";
import subscriptionPlanService from "@/services/subscription/subscriptionPlanService";
import hospitalSubscriptionService from "@/services/subscription/hospitalSubscriptionService";

import "./HospitalSubscriptionForm.css";

export default function HospitalSubscriptionForm({
    initialData = null,
    isEdit = false,
}) {

    const router = useRouter();

    const [hospitals, setHospitals] = useState([]);
    const [plans, setPlans] = useState([]);

    const [loading, setLoading] = useState(false);
    const [loadingData, setLoadingData] = useState(true);

    const [formData, setFormData] = useState({

        hospitalId: initialData?.hospitalId || "",

        subscriptionPlanId:
            initialData?.subscriptionPlanId || "",

        startDate:
            initialData?.startDate
                ? initialData.startDate.split("T")[0]
                : "",

        endDate:
            initialData?.endDate
                ? initialData.endDate.split("T")[0]
                : "",

        trialEndsOn:
            initialData?.trialEndsOn
                ? initialData.trialEndsOn.split("T")[0]
                : "",

        amountPaid:
            initialData?.amountPaid ?? "",

        paymentStatus:
            initialData?.paymentStatus || "Paid",

    });

    useEffect(() => {

        loadData();

    }, []);

    const loadData = async () => {

        try {

            const [hospitalResponse, planResponse] =
                await Promise.all([
                    hospitalService.getHospitals({
                        page: 1,
                        pageSize: 100,
                        search: "",
                        status: "",
                    }),
                    subscriptionPlanService.getSubscriptionPlans(),
                ]);

            if (hospitalResponse.success) {

                setHospitals(
                    hospitalResponse.data.items || []
                );

            }

            if (planResponse.success) {

                setPlans(
                    planResponse.data || []
                );

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Failed to load subscription data.");

        }
        finally {

            setLoadingData(false);

        }

    };

    const handleChange = (e) => {

        const { name, value } = e.target;

        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));

    };
    const handlePlanChange = (e) => {

        const planId = e.target.value;

        const selectedPlan = plans.find(
            (plan) => plan.id === planId
        );

        if (!selectedPlan) {

            setFormData((prev) => ({
                ...prev,
                subscriptionPlanId: "",
                endDate: "",
                trialEndsOn: "",
                amountPaid: "",
            }));

            return;
        }

        const startDate = new Date();

        const endDate = new Date(startDate);

        endDate.setDate(
            endDate.getDate() + selectedPlan.durationInDays
        );

        const formatDate = (date) => {

            return date.toISOString().split("T")[0];

        };

        setFormData((prev) => ({

            ...prev,

            subscriptionPlanId: planId,

            startDate: formatDate(startDate),

            endDate: formatDate(endDate),

            amountPaid: selectedPlan.price,

            trialEndsOn: selectedPlan.isTrial
                ? formatDate(endDate)
                : "",

        }));

    };

    const handleSubmit = async (e) => {

        e.preventDefault();

        if (!formData.hospitalId) {

            toast.error("Please select a hospital.");

            return;

        }

        if (!formData.subscriptionPlanId) {

            toast.error("Please select a subscription plan.");

            return;

        }

        if (!formData.startDate) {

            toast.error("Start date is required.");

            return;

        }

        if (!formData.endDate) {

            toast.error("End date is required.");

            return;

        }

        try {

            setLoading(true);

            let response;

            if (isEdit) {

                response =
                    await hospitalSubscriptionService.updateSubscription(
                        initialData.id,
                        formData
                    );

            }
            else {

                response =
                    await hospitalSubscriptionService.createSubscription(
                        formData
                    );

            }

            if (response.success) {

                toast.success(
                    isEdit
                        ? "Subscription updated successfully."
                        : "Subscription created successfully."
                );

                router.push("/super-admin/subscriptions");

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

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
                        Subscription Information
                    </h5>

                    <div className="row">

                        {/* Hospital */}

                        <div className="col-md-6 mb-3">

                            <label className="form-label">
                                Hospital
                            </label>

                            <select
                                className="form-select"
                                name="hospitalId"
                                value={formData.hospitalId}
                                onChange={handleChange}
                                disabled={loadingData || isEdit}
                            >

                                <option value="">
                                    Select Hospital
                                </option>

                                {hospitals.map((hospital) => (

                                    <option
                                        key={hospital.hospitalId}
                                        value={hospital.hospitalId}
                                    >
                                        {hospital.name}
                                    </option>

                                ))}

                            </select>

                        </div>


                        {/* Subscription Plan */}

                        <div className="col-md-6 mb-3">

                            <label className="form-label">
                                Subscription Plan
                            </label>

                            <select
                                className="form-select"
                                name="subscriptionPlanId"
                                value={formData.subscriptionPlanId}
                                onChange={handlePlanChange}
                                disabled={loadingData}
                            >
                                <option value="">
                                    Select Plan
                                </option>

                                {plans.map((plan) => (

                                    <option
                                        key={plan.id}
                                        value={plan.id}
                                    >
                                        {plan.name} - ₹{plan.price}
                                    </option>

                                ))}
                            </select>

                        </div>

                        {/* Start Date */}

                        <div className="col-md-4 mb-3">

                            <label className="form-label">
                                Start Date
                            </label>

                            <input
                                type="date"
                                className="form-control"
                                name="startDate"
                                value={formData.startDate}
                                onChange={handleChange}
                            />

                        </div>


                        {/* End Date */}

                        <div className="col-md-4 mb-3">

                            <label className="form-label">
                                End Date
                            </label>

                            <input
                                type="date"
                                className="form-control"
                                name="endDate"
                                value={formData.endDate}
                                onChange={handleChange}
                            />

                        </div>


                        {/* Trial Ends On */}

                        <div className="col-md-4 mb-3">

                            <label className="form-label">
                                Trial Ends On
                            </label>

                            <input
                                type="date"
                                className="form-control"
                                name="trialEndsOn"
                                value={formData.trialEndsOn}
                                onChange={handleChange}
                                disabled={
                                    !plans.find(
                                        (plan) =>
                                            plan.id === formData.subscriptionPlanId
                                    )?.isTrial
                                }
                            />

                        </div>

                        {/* Amount Paid */}

                        <div className="col-md-6 mb-3">

                            <label className="form-label">
                                Amount Paid
                            </label>

                            <input
                                type="number"
                                className="form-control"
                                name="amountPaid"
                                value={formData.amountPaid}
                                onChange={handleChange}
                                min="0"
                                step="0.01"
                            />

                        </div>


                        {/* Payment Status */}

                        <div className="col-md-6 mb-3">

                            <label className="form-label">
                                Payment Status
                            </label>

                            <select
                                className="form-select"
                                name="paymentStatus"
                                value={formData.paymentStatus}
                                onChange={handleChange}
                            >

                                <option value="Paid">
                                    Paid
                                </option>

                                <option value="Pending">
                                    Pending
                                </option>

                                <option value="Failed">
                                    Failed
                                </option>

                                <option value="Free">
                                    Free
                                </option>

                            </select>

                        </div>
                    </div>

                </div>


                <div className="form-actions">

                    <button
                        type="button"
                        className="btn btn-light"
                        onClick={() =>
                            router.push("/super-admin/subscriptions")
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
                            : (isEdit ? "Update Subscription" : "Save Subscription")
                        }
                    </button>

                </div>
            </form>

        </div>

    );

}