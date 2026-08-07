"use client";

import { useEffect, useState } from "react";
import { useParams } from "next/navigation";

import hospitalService from "@/services/hospital/hospitalService";
import HospitalForm from "@/components/super-admin/hospital/HospitalForm";

export default function EditHospitalPage() {

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

console.log(response);

if (response.success) {

    console.log(response.data);

    setHospital(response.data);

}
           

        } catch (error) {

            console.error(error);

        } finally {

            setLoading(false);

        }

    };

    if (loading) {

        return <p>Loading...</p>;

    }

    return (

        <HospitalForm
            initialData={hospital}
            isEdit={true}
        />

    );

}