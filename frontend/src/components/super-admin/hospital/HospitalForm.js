"use client";

import { useRouter } from "next/navigation";
import { toast } from "react-toastify";
import states from "@/data/states";
import { useState, useEffect } from "react";
import hospitalService from "@/services/hospital/hospitalService";

import "./HospitalForm.css";

export default function HospitalForm({

    initialData = null,

    isEdit = false,

}) {

    const router = useRouter();

    const [loading, setLoading] = useState(false);

    const [formData, setFormData] = useState({

        name: initialData?.name || "",
        code: initialData?.code || "",
        hospitalType: initialData?.hospitalType || "",
        licenseNumber: initialData?.licenseNumber || "",
        gstNumber: initialData?.gstNumber || "",
        email: initialData?.email || "",
        phoneNumber: initialData?.phoneNumber || "",
        website: initialData?.website || "",
        address: initialData?.address || "",
        city: initialData?.city || "",
        state: initialData?.state || "",
        country: initialData?.country || "India",
        postalCode: initialData?.postalCode || "",

    });
    useEffect(() => {

        if (!initialData) return;

        setFormData({

            name: initialData.name || "",
            code: initialData.code || "",
            hospitalType: initialData.hospitalType || "",
            licenseNumber: initialData.licenseNumber || "",
            gstNumber: initialData.gstNumber || "",
            email: initialData.email || "",
            phoneNumber: initialData.phoneNumber || "",
            website: initialData.website || "",
            address: initialData.address || "",
            city: initialData.city || "",
            state: initialData.state || "",
            country: initialData.country || "India",
            postalCode: initialData.postalCode || "",

        });

    }, [initialData]);
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

            let response;

            if (isEdit) {

                response = await hospitalService.updateHospital(

                    initialData.hospitalId,

                    formData

                );

            }
            else {

                response = await hospitalService.createHospital(formData);

            }
            if (response.success) {

                toast.success(

                    isEdit

                        ? "Hospital updated successfully."

                        : "Hospital created successfully."

                );
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
                                value={isEdit ? formData.code : "Auto Generated"}
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

                            <select
                                className="form-select"
                                name="state"
                                value={formData.state}
                                onChange={handleChange}
                            >
                                <option value="">Select State</option>

                                {states.map((state) => (
                                    <option key={state} value={state}>
                                        {state}
                                    </option>
                                ))}
                            </select>

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

                        {
                            loading
                                ? (isEdit ? "Updating..." : "Saving...")
                                : (isEdit ? "Update Hospital" : "Save Hospital")
                        }

                    </button>

                </div>

            </form>

        </div>

    );

}