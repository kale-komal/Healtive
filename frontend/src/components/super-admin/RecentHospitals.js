"use client";

import { useEffect, useState } from "react";

import hospitalService from "@/services/hospital/hospitalService";

import "./RecentHospitals.css";

export default function RecentHospitals() {

    const [hospitals, setHospitals] = useState([]);

    const [loading, setLoading] = useState(true);

    useEffect(() => {

        loadHospitals();

    }, []);

    const loadHospitals = async () => {

        try {

            const response = await hospitalService.getHospitals();

            if (response.success) {

                setHospitals(response.data);

            }

        }
        catch (error) {

            console.log(error);

        }
        finally {

            setLoading(false);

        }

    };

    if (loading) {

        return <p>Loading hospitals...</p>;

    }

    if (hospitals.length === 0) {

        return <p>No hospitals found.</p>;

    }

    return (

        <table className="recent-hospitals-table">

            <thead>

                <tr>

                    <th>Hospital</th>

                    <th>Code</th>

                    <th>Status</th>

                    <th>Created</th>

                </tr>

            </thead>

            <tbody>

                {

                    hospitals.map((hospital) => (

                        <tr key={hospital.hospitalId}>

                            <td>

                                <div className="hospital-name">

                                    <strong>{hospital.name}</strong>

                                    <small>{hospital.email}</small>

                                </div>

                            </td>

                            <td>{hospital.code}</td>

                            <td>

                                <span
                                    className={
                                        hospital.isActive
                                            ? "status active"
                                            : "status inactive"
                                    }
                                >

                                    {

                                        hospital.isActive
                                            ? "Active"
                                            : "Inactive"

                                    }

                                </span>

                            </td>

                            <td>

                                {
                                    new Date(
                                        hospital.createdAt
                                    ).toLocaleDateString()
                                }

                            </td>

                        </tr>

                    ))

                }

            </tbody>

        </table>

    );

}