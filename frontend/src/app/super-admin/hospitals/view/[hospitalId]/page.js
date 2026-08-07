"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";

import hospitalService from "@/services/hospital/hospitalService";
import HospitalView from "@/components/super-admin/hospital/HospitalView";

export default function ViewHospitalPage() {

    const { hospitalId } = useParams();

    const [hospital, setHospital] = useState(null);

    const [loading, setLoading] = useState(true);

    useEffect(() => {

        loadHospital();

    }, []);

    const loadHospital = async () => {

        try {

            const response =
                await hospitalService.getHospitalById(hospitalId);

            if (response.success) {

                setHospital(response.data);

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

        return <p>Loading...</p>;

    }

    return <HospitalView hospital={hospital} />;

}