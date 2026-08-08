"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";

import hospitalSubscriptionService
    from "@/services/subscription/hospitalSubscriptionService";

import HospitalSubscriptionForm
    from "@/components/super-admin/subscription/HospitalSubscriptionForm";

export default function EditSubscriptionPage() {

    const { id } = useParams();

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

            console.log("Subscription:", response);

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

        <HospitalSubscriptionForm
            initialData={subscription}
            isEdit={true}
        />

    );

}