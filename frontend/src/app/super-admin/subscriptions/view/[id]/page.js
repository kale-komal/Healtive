"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";

import hospitalSubscriptionService from "@/services/subscription/hospitalSubscriptionService";

export default function ViewSubscriptionPage() {

    const { id } = useParams();

    const router = useRouter();

    const [subscription, setSubscription] = useState(null);

    const [loading, setLoading] = useState(true);

    useEffect(() => {

        if (id) {
            loadSubscription();
        }

    }, [id]);

    const loadSubscription = async () => {

        try {

            const response =
                await hospitalSubscriptionService.getSubscriptionById(id);

            console.log(response);

            if (response.success) {

                setSubscription(response.data);

            }

        }
        catch (error) {

            console.error(error);

        }
        finally {

            setLoading(false);

        }

    };

    if (loading) {

        return <p>Loading subscription...</p>;

    }

    if (!subscription) {

        return <p>Subscription not found.</p>;

    }

    return (

        <div className="container-fluid">

            <div className="card">

                <div className="card-header">

                    <h4>
                        Subscription Details
                    </h4>

                </div>

                <div className="card-body">

                    <div className="row">

                        <div className="col-md-6 mb-3">

                            <strong>
                                Hospital ID
                            </strong>

                            <p>
                                {subscription.hospitalId}
                            </p>

                        </div>

                        <div className="col-md-6 mb-3">

                            <strong>
                                Plan ID
                            </strong>

                            <p>
                                {subscription.subscriptionPlanId}
                            </p>

                        </div>

                        <div className="col-md-4 mb-3">

                            <strong>
                                Start Date
                            </strong>

                            <p>
                                {new Date(
                                    subscription.startDate
                                ).toLocaleDateString()}
                            </p>

                        </div>

                        <div className="col-md-4 mb-3">

                            <strong>
                                End Date
                            </strong>

                            <p>
                                {new Date(
                                    subscription.endDate
                                ).toLocaleDateString()}
                            </p>

                        </div>

                        <div className="col-md-4 mb-3">

                            <strong>
                                Amount Paid
                            </strong>

                            <p>
                                ₹{subscription.amountPaid}
                            </p>

                        </div>

                        <div className="col-md-4 mb-3">

                            <strong>
                                Payment Status
                            </strong>

                            <p>
                                {subscription.paymentStatus}
                            </p>

                        </div>

                        <div className="col-md-4 mb-3">

                            <strong>
                                Status
                            </strong>

                            <p>

                                {subscription.isActive
                                    ? "Active"
                                    : "Inactive"}

                            </p>

                        </div>

                    </div>

                    <button
                        className="btn btn-light"
                        onClick={() =>
                            router.push(
                                "/super-admin/subscriptions"
                            )
                        }
                    >
                        Back
                    </button>

                </div>

            </div>

        </div>

    );

}