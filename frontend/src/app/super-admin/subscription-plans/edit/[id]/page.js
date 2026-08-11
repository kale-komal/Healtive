"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";
import { toast } from "react-toastify";

import subscriptionPlanService
    from "@/services/subscription/subscriptionPlanService";

import SubscriptionPlanForm
    from "@/components/super-admin/subscription/SubscriptionPlanForm";

export default function EditSubscriptionPlanPage() {

    const { id } = useParams();

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
                await subscriptionPlanService
                    .getSubscriptionPlanById(id);

            console.log("Edit Plan Response:", response);

            if (response.success) {

                setPlan(response.data);

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error(error);

            toast.error(
                "Failed to load subscription plan."
            );

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

        <SubscriptionPlanForm
            initialData={plan}
            isEdit={true}
        />

    );

}