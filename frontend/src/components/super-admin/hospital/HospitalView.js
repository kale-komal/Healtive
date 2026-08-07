"use client";

import { useRouter } from "next/navigation";

import "./HospitalView.css";

export default function HospitalView({ hospital }) {

    const router = useRouter();

    return (

        <div className="view-card">

            <div className="view-header">

                <h3>Hospital Details</h3>

                <button
                    className="btn btn-secondary"
                    onClick={() => router.back()}
                >
                    Back
                </button>

            </div>

            {/* Hospital Information */}

            <div className="view-section">

                <h5>Hospital Information</h5>

                <div className="row">

                    <div className="col-md-6">
                        <p><strong>Hospital Name:</strong> {hospital.name}</p>
                    </div>

                    <div className="col-md-6">
                        <p><strong>Hospital Code:</strong> {hospital.code}</p>
                    </div>

                    <div className="col-md-6">
                        <p><strong>Hospital Type:</strong> {hospital.hospitalType}</p>
                    </div>

                    <div className="col-md-6">
                        <p><strong>License Number:</strong> {hospital.licenseNumber || "-"}</p>
                    </div>

                    <div className="col-md-6">
                        <p><strong>GST Number:</strong> {hospital.gstNumber || "-"}</p>
                    </div>

                </div>

            </div>

            {/* Contact */}

            <div className="view-section">

                <h5>Contact Information</h5>

                <div className="row">

                    <div className="col-md-6">
                        <p><strong>Email:</strong> {hospital.email}</p>
                    </div>

                    <div className="col-md-6">
                        <p><strong>Phone:</strong> {hospital.phoneNumber}</p>
                    </div>

                    <div className="col-md-12">
                        <p><strong>Website:</strong> {hospital.website || "-"}</p>
                    </div>

                </div>

            </div>

            {/* Address */}

            <div className="view-section">

                <h5>Address Information</h5>

                <div className="row">

                    <div className="col-md-12">
                        <p><strong>Address:</strong> {hospital.address}</p>
                    </div>

                    <div className="col-md-4">
                        <p><strong>City:</strong> {hospital.city}</p>
                    </div>

                    <div className="col-md-4">
                        <p><strong>State:</strong> {hospital.state}</p>
                    </div>

                    <div className="col-md-4">
                        <p><strong>Country:</strong> {hospital.country}</p>
                    </div>

                    <div className="col-md-4">
                        <p><strong>Postal Code:</strong> {hospital.postalCode}</p>
                    </div>

                </div>

            </div>

            {/* System */}

            <div className="view-section">

                <h5>System Information</h5>

                <div className="row">

                    <div className="col-md-4">
                        <p><strong>Time Zone:</strong> {hospital.timeZone}</p>
                    </div>

                    <div className="col-md-4">
                        <p><strong>Currency:</strong> {hospital.currency}</p>
                    </div>

                    <div className="col-md-4">
                        <p>
                            <strong>Status:</strong>{" "}
                            {hospital.isActive
                                ? <span className="badge bg-success">Active</span>
                                : <span className="badge bg-danger">Inactive</span>}
                        </p>
                    </div>

                </div>

            </div>

        </div>

    );

}