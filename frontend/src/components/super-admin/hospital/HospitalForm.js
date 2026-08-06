"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { toast } from "react-toastify";

import hospitalService from "@/services/hospital/hospitalService";

import "./HospitalForm.css";

export default function HospitalForm() {

    const router = useRouter();

    const [loading, setLoading] = useState(false);

    const [formData, setFormData] = useState({

        name: "",
        hospitalType: "",
        licenseNumber: "",
        gstNumber: "",
        email: "",
        phoneNumber: "",
        website: "",
        address: "",
        city: "",
        state: "",
        country: "India",
        postalCode: "",

    });

    const handleChange = (e) => {

        const { name, value } = e.target;

        setFormData((prev) => ({

            ...prev,

            [name]: value,

        }));

    };

    const handleSubmit = async (e) => {

        e.preventDefault();

        if (!formData.name.trim()) {

            toast.error("Hospital Name is required.");

            return;

        }

        

        if (!formData.hospitalType) {

            toast.error("Hospital Type is required.");

            return;

        }

        if (!formData.email.trim()) {

            toast.error("Email is required.");

            return;

        }

        if (!formData.phoneNumber.trim()) {

            toast.error("Phone Number is required.");

            return;

        }

        try {

            setLoading(true);

            const response = await hospitalService.createHospital(formData);

            if (response.success) {

                toast.success(response.message);

                router.push("/super-admin/hospitals");

            } else {

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

                {/* Hospital Information */}

                <div className="form-section">

                    <h5>Hospital Information</h5>

                    <div className="row">

                        <div className="col-md-6 mb-3">

                            <label className="form-label">

                                Hospital Name

                            </label>

                            <input
                                autoFocus
                                type="text"
                                className="form-control"
                                name="name"
                                value={formData.name}
                                onChange={handleChange}
                                placeholder="Enter hospital name"
                            />

                        </div>

                        <div className="col-md-6 mb-3">

                            <label className="form-label">

                                Hospital Code

                            </label>

                            <input
    className="form-control"
    value="Auto Generated"
    readOnly
/>

                        </div>

                        <div className="col-md-6 mb-3">

                            <label className="form-label">

                                Hospital Type

                            </label>

                            <select
                                className="form-select"
                                name="hospitalType"
                                value={formData.hospitalType}
                                onChange={handleChange}
                            >

                                <option value="">Select Hospital Type</option>
                                <option value="General">General</option>
                                <option value="Speciality">Speciality</option>
                                <option value="Multi Speciality">Multi Speciality</option>
                                <option value="Clinic">Clinic</option>

                            </select>

                        </div>

                        <div className="col-md-6 mb-3">

                            <label className="form-label">

                                License Number

                            </label>

                            <input
                                type="text"
                                className="form-control"
                                name="licenseNumber"
                                value={formData.licenseNumber}
                                onChange={handleChange}
                            />

                        </div>

                        <div className="col-md-6 mb-3">

                            <label className="form-label">

                                GST Number

                            </label>

                            <input
                                type="text"
                                className="form-control"
                                name="gstNumber"
                                value={formData.gstNumber}
                                onChange={handleChange}
                            />

                        </div>

                    </div>

                </div>

                {/* Contact */}

                <div className="form-section">

                    <h5>Contact Information</h5>

                    <div className="row">

                        <div className="col-md-6 mb-3">

                            <label className="form-label">

                                Email

                            </label>

                            <input
                                type="email"
                                className="form-control"
                                name="email"
                                value={formData.email}
                                onChange={handleChange}
                            />

                        </div>

                        <div className="col-md-6 mb-3">

                            <label className="form-label">

                                Phone Number

                            </label>

                            <input
                                type="text"
                                className="form-control"
                                name="phoneNumber"
                                value={formData.phoneNumber}
                                onChange={handleChange}
                                maxLength={10}
                            />

                        </div>

                        <div className="col-md-12 mb-3">

                            <label className="form-label">

                                Website

                            </label>

                            <input
                                type="url"
                                className="form-control"
                                name="website"
                                value={formData.website}
                                onChange={handleChange}
                            />

                        </div>

                    </div>

                </div>

                {/* Address */}

                <div className="form-section">

                    <h5>Address Information</h5>

                    <div className="row">

                        <div className="col-md-12 mb-3">

                            <label className="form-label">

                                Address

                            </label>

                            <textarea
                                className="form-control"
                                rows="3"
                                name="address"
                                value={formData.address}
                                onChange={handleChange}
                            />

                        </div>

                        <div className="col-md-4 mb-3">

                            <label className="form-label">

                                City

                            </label>

                            <input
                                type="text"
                                className="form-control"
                                name="city"
                                value={formData.city}
                                onChange={handleChange}
                            />

                        </div>

                        <div className="col-md-4 mb-3">

                            <label className="form-label">

                                State

                            </label>

                            <input
                                type="text"
                                className="form-control"
                                name="state"
                                value={formData.state}
                                onChange={handleChange}
                            />

                        </div>

                        <div className="col-md-4 mb-3">

                            <label className="form-label">

                                Country

                            </label>

                            <input
                                type="text"
                                className="form-control"
                                name="country"
                                value={formData.country}
                                readOnly
                            />

                        </div>

                        <div className="col-md-4 mb-3">

                            <label className="form-label">

                                Postal Code

                            </label>

                            <input
                                type="text"
                                className="form-control"
                                name="postalCode"
                                value={formData.postalCode}
                                onChange={handleChange}
                                maxLength={6}
                            />

                        </div>

                    </div>

                </div>

                <div className="form-actions">

                    <button
                        type="button"
                        className="btn btn-light"
                        onClick={() => router.push("/super-admin/hospitals")}
                    >

                        Cancel

                    </button>

                    <button
                        type="submit"
                        className="btn btn-primary"
                        disabled={loading}
                    >

                        {loading ? "Saving..." : "Save Hospital"}

                    </button>

                </div>

            </form>

        </div>

    );

}