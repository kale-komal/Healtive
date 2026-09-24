"use client";

import { useEffect, useState } from "react";

import {
    Edit3,
    Save,
    X,
    Loader2,
    Info,
} from "lucide-react";

import { toast } from "react-toastify";

import hospitalAdminService from "@/services/hospital-admin/hospitalAdminService";
import DashboardSection from "@/components/super-admin/DashboardSection";
import PageHeader from "@/components/super-admin/PageHeader";
import states from "@/data/states";

import "./Profile.css";

const normalizeResponse = (data) => {

    // Backend returns the { success, message, data, errors } envelope.
    if (
        data &&
        typeof data === "object" &&
        !Array.isArray(data) &&
        "success" in data
    ) {

        return {
            ok: !!data.success,
            message: data.message,
            data: data.success ? data.data : null,
        };

    }

    return {
        ok: data !== null && data !== undefined,
        message: "",
        data,
    };

};

const HOSPITAL_TYPES = [
    "General",
    "Speciality",
    "Multi Speciality",
    "Clinic",
];

const TIME_ZONES = [
    "Asia/Kolkata",
    "Asia/Riyadh",
    "Asia/Dubai",
    "Asia/Karachi",
    "Asia/Singapore",
    "Asia/Tokyo",
    "Europe/London",
    "Europe/Paris",
    "America/New_York",
    "America/Chicago",
    "America/Los_Angeles",
    "Australia/Sydney",
    "UTC",
];

const CURRENCIES = ["INR", "USD", "EUR", "GBP", "AED", "SAR"];

const formatDate = (value) => {

    if (!value) return "—";

    const date = new Date(value);

    if (Number.isNaN(date.getTime())) return "—";

    return date.toLocaleDateString("en-IN", {
        day: "2-digit",
        month: "short",
        year: "numeric",
    });

};

const toProfileForm = (profile) => ({

    name: profile?.name || "",
    hospitalType: profile?.hospitalType || "",
    licenseNumber: profile?.licenseNumber || "",
    gstNumber: profile?.gstNumber || "",
    email: profile?.email || "",
    phoneNumber: profile?.phoneNumber || "",
    website: profile?.website || "",
    address: profile?.address || "",
    city: profile?.city || "",
    state: profile?.state || "",
    country: profile?.country || "India",
    postalCode: profile?.postalCode || "",
    timeZone: profile?.timeZone || "Asia/Kolkata",
    currency: profile?.currency || "INR",

});

const buildPayload = (form) => ({

    name: form.name.trim(),
    licenseNumber: form.licenseNumber.trim(),
    gstNumber: form.gstNumber.trim(),
    hospitalType: form.hospitalType,
    email: form.email.trim(),
    phoneNumber: form.phoneNumber.trim(),
    website: form.website.trim(),
    address: form.address.trim(),
    city: form.city.trim(),
    state: form.state,
    country: form.country.trim() || "India",
    postalCode: form.postalCode.trim(),
    timeZone: form.timeZone,
    currency: form.currency,

});

const ViewItem = ({ label, value }) => (

    <div className="profile-view-item">

        <span className="profile-view-label">{label}</span>

        <span className="profile-view-value">{value || "—"}</span>

    </div>

);

export default function HospitalAdminProfilePage() {

    const [mode, setMode] = useState("view");

    const [profile, setProfile] = useState(null);

    const [form, setForm] = useState(toProfileForm(null));

    const [loading, setLoading] = useState(true);

    const [saving, setSaving] = useState(false);

    const [notFound, setNotFound] = useState(false);

    useEffect(() => {

        (async () => {

            try {

                const response = await hospitalAdminService.getProfile();

                const normalized = normalizeResponse(response);

                if (!normalized.ok) {

                    toast.error(
                        normalized.message || "Failed to load hospital profile."
                    );

                    return;

                }

                if (!normalized.data) {

                    setNotFound(true);

                    return;

                }

                setProfile(normalized.data);

            }
            catch (error) {

                console.error(error);

                toast.error(
                    "Something went wrong while loading the profile."
                );

            }
            finally {

                setLoading(false);

            }

        })();

    }, []);

    const startEditing = () => {

        setForm(toProfileForm(profile));

        setMode("edit");

    };

    const cancelEditing = () => {

        setForm(toProfileForm(profile));

        setMode("view");

    };

    const handleChange = (e) => {

        const { name, value } = e.target;

        setForm((prev) => ({ ...prev, [name]: value }));

    };

    const handleSave = async () => {

        if (!form.name.trim()) {

            toast.error("Hospital name is required.");

            return;

        }

        if (!form.hospitalType) {

            toast.error("Hospital type is required.");

            return;

        }

        if (!form.email.trim()) {

            toast.error("Email is required.");

            return;

        }

        if (!form.phoneNumber.trim() || form.phoneNumber.trim().length !== 10) {

            toast.error("Phone number must be exactly 10 digits.");

            return;

        }

        setSaving(true);

        try {

            const response = await hospitalAdminService.updateProfile(
                buildPayload(form)
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to update hospital profile."
                );

                return;

            }

            const updated = {
                ...profile,
                ...buildPayload(form),
                hospitalId: profile?.hospitalId,
                code: profile?.code,
                isActive: profile?.isActive,
                createdAt: profile?.createdAt,
            };

            setProfile(updated);

            setForm(toProfileForm(updated));

            setMode("view");

            toast.success("Hospital updated successfully.");

        }
        catch (error) {

            console.error(error);

            toast.error(
                "Something went wrong while updating the profile."
            );

        }
        finally {

            setSaving(false);

        }

    };

    if (loading) {

        return (

            <div className="profile-loading">

                <Loader2 className="profile-spin" size={22} />

                <span>Loading hospital profile...</span>

            </div>

        );

    }

    if (notFound || !profile) {

        return (

            <div className="profile-empty">

                <Info size={40} />

                <h4>No hospital profile</h4>

                <p>
                    {
                        notFound
                            ? "The server did not return hospital information."
                            : "Unable to load hospital information."
                    }
                </p>

            </div>

        );

    }

    if (mode === "edit") {

        return (

            <div>

                <PageHeader
                    title="Edit Hospital Profile"
                    subtitle="Update your hospital information and save changes."
                />

                <div className="profile-form-card">

                    <form
                        onSubmit={(e) => {

                            e.preventDefault();

                            handleSave();

                        }}
                    >

                        {/* Hospital Information */}

                        <div className="profile-form-section">

                            <h5>Hospital Information</h5>

                            <div className="row g-3">

                                <div className="col-md-6">

                                    <label className="form-label">
                                        Hospital Name
                                    </label>

                                    <input
                                        autoFocus
                                        type="text"
                                        className="form-control"
                                        name="name"
                                        value={form.name}
                                        onChange={handleChange}
                                        placeholder="Enter hospital name"
                                    />

                                </div>

                                <div className="col-md-6">

                                    <label className="form-label">
                                        Hospital Code
                                    </label>

                                    <input
                                        className="form-control"
                                        value={profile?.code || "—"}
                                        readOnly
                                    />

                                </div>

                                <div className="col-md-6">

                                    <label className="form-label">
                                        Hospital Type
                                    </label>

                                    <select
                                        className="form-select"
                                        name="hospitalType"
                                        value={form.hospitalType}
                                        onChange={handleChange}
                                    >

                                        <option value="">
                                            Select Hospital Type
                                        </option>

                                        {HOSPITAL_TYPES.map((type) => (

                                            <option
                                                key={type}
                                                value={type}
                                            >
                                                {type}
                                            </option>

                                        ))}

                                    </select>

                                </div>

                                <div className="col-md-6">

                                    <label className="form-label">
                                        License Number
                                    </label>

                                    <input
                                        type="text"
                                        className="form-control"
                                        name="licenseNumber"
                                        value={form.licenseNumber}
                                        onChange={handleChange}
                                    />

                                </div>

                                <div className="col-md-6">

                                    <label className="form-label">
                                        GST Number
                                    </label>

                                    <input
                                        type="text"
                                        className="form-control"
                                        name="gstNumber"
                                        value={form.gstNumber}
                                        onChange={handleChange}
                                    />

                                </div>

                            </div>

                        </div>

                        {/* Contact */}

                        <div className="profile-form-section">

                            <h5>Contact Information</h5>

                            <div className="row g-3">

                                <div className="col-md-6">

                                    <label className="form-label">
                                        Email
                                    </label>

                                    <input
                                        type="email"
                                        className="form-control"
                                        name="email"
                                        value={form.email}
                                        onChange={handleChange}
                                    />

                                </div>

                                <div className="col-md-6">

                                    <label className="form-label">
                                        Phone Number
                                    </label>

                                    <input
                                        type="text"
                                        className="form-control"
                                        name="phoneNumber"
                                        value={form.phoneNumber}
                                        onChange={handleChange}
                                        maxLength={10}
                                    />

                                </div>

                                <div className="col-12">

                                    <label className="form-label">
                                        Website
                                    </label>

                                    <input
                                        type="url"
                                        className="form-control"
                                        name="website"
                                        value={form.website}
                                        onChange={handleChange}
                                        placeholder="https://example.com"
                                    />

                                </div>

                            </div>

                        </div>

                        {/* Address */}

                        <div className="profile-form-section">

                            <h5>Address Information</h5>

                            <div className="row g-3">

                                <div className="col-12">

                                    <label className="form-label">
                                        Address
                                    </label>

                                    <textarea
                                        rows="3"
                                        className="form-control"
                                        name="address"
                                        value={form.address}
                                        onChange={handleChange}
                                    />

                                </div>

                                <div className="col-md-4">

                                    <label className="form-label">
                                        City
                                    </label>

                                    <input
                                        type="text"
                                        className="form-control"
                                        name="city"
                                        value={form.city}
                                        onChange={handleChange}
                                    />

                                </div>

                                <div className="col-md-4">

                                    <label className="form-label">
                                        State
                                    </label>

                                    <select
                                        className="form-select"
                                        name="state"
                                        value={form.state}
                                        onChange={handleChange}
                                    >

                                        <option value="">Select State</option>

                                        {states.map((state) => (

                                            <option
                                                key={state}
                                                value={state}
                                            >
                                                {state}
                                            </option>

                                        ))}

                                    </select>

                                </div>

                                <div className="col-md-4">

                                    <label className="form-label">
                                        Country
                                    </label>

                                    <input
                                        type="text"
                                        className="form-control"
                                        value={form.country || "India"}
                                        readOnly
                                    />

                                </div>

                                <div className="col-md-4">

                                    <label className="form-label">
                                        Postal Code
                                    </label>

                                    <input
                                        type="text"
                                        className="form-control"
                                        name="postalCode"
                                        value={form.postalCode}
                                        onChange={handleChange}
                                        maxLength={6}
                                    />

                                </div>

                            </div>

                        </div>

                        {/* System */}

                        <div className="profile-form-section">

                            <h5>System Information</h5>

                            <div className="row g-3">

                                <div className="col-md-6">

                                    <label className="form-label">
                                        Time Zone
                                    </label>

                                    <select
                                        className="form-select"
                                        name="timeZone"
                                        value={form.timeZone}
                                        onChange={handleChange}
                                    >

                                        {TIME_ZONES.map((zone) => (

                                            <option
                                                key={zone}
                                                value={zone}
                                            >
                                                {zone}
                                            </option>

                                        ))}

                                    </select>

                                </div>

                                <div className="col-md-6">

                                    <label className="form-label">
                                        Currency
                                    </label>

                                    <select
                                        className="form-select"
                                        name="currency"
                                        value={form.currency}
                                        onChange={handleChange}
                                    >

                                        {CURRENCIES.map((currency) => (

                                            <option
                                                key={currency}
                                                value={currency}
                                            >
                                                {currency}
                                            </option>

                                        ))}

                                    </select>

                                </div>

                                <div className="col-md-6">

                                    <label className="form-label">
                                        Status
                                    </label>

                                    <div>

                                        <span
                                            className={
                                                profile?.isActive
                                                    ? "profile-status profile-status-active"
                                                    : "profile-status profile-status-inactive"
                                            }
                                        >

                                            {profile?.isActive
                                                ? "Active"
                                                : "Inactive"}

                                        </span>

                                    </div>

                                </div>

                                <div className="col-md-6">

                                    <label className="form-label">
                                        Created At
                                    </label>

                                    <div className="profile-form-readonly">
                                        {formatDate(profile?.createdAt)}
                                    </div>

                                </div>

                            </div>

                        </div>

                        <div className="profile-form-actions">

                            <button
                                type="button"
                                className="btn btn-light"
                                onClick={cancelEditing}
                            >

                                <X size={16} />

                                <span>Cancel</span>

                            </button>

                            <button
                                type="submit"
                                className="btn btn-primary"
                                disabled={saving}
                            >

                                {
                                    saving
                                        ? (
                                            <><Loader2
                                                className="profile-spin"
                                                size={16}
                                            /><span>Saving...</span></>
                                        )
                                        : (
                                            <><Save
                                                size={16}
                                            /><span>Save Changes</span></>
                                        )
                                }

                            </button>

                        </div>

                    </form>

                </div>

            </div>

        );

    }

    return (

        <div>

            <PageHeader
                title="Hospital Profile"
                subtitle="View and manage your hospital information."
                action={

                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={startEditing}
                    >

                        <Edit3 size={16} />

                        <span>Edit Profile</span>

                    </button>

                }
            />

            <div className="profile-view-grid">

                <DashboardSection title="Hospital Information">

                    <ViewItem label="Hospital Name" value={profile?.name} />

                    <ViewItem label="Hospital Code" value={profile?.code} />

                    <ViewItem label="Hospital Type" value={profile?.hospitalType} />

                    <ViewItem label="License Number" value={profile?.licenseNumber} />

                    <ViewItem label="GST Number" value={profile?.gstNumber} />

                </DashboardSection>

                <DashboardSection title="Contact Information">

                    <ViewItem label="Email" value={profile?.email} />

                    <ViewItem label="Phone Number" value={profile?.phoneNumber} />

                    <ViewItem label="Website" value={profile?.website} />

                </DashboardSection>

                <DashboardSection title="Address Information">

                    <ViewItem label="Address" value={profile?.address} />

                    <ViewItem label="City" value={profile?.city} />

                    <ViewItem label="State" value={profile?.state} />

                    <ViewItem label="Country" value={profile?.country} />

                    <ViewItem label="Postal Code" value={profile?.postalCode} />

                </DashboardSection>

                <DashboardSection title="System Information">

                    <ViewItem label="Time Zone" value={profile?.timeZone} />

                    <ViewItem label="Currency" value={profile?.currency} />

                    <ViewItem
                        label="Status"
                        value={

                            <span
                                className={
                                    profile?.isActive
                                        ? "profile-status profile-status-active"
                                        : "profile-status profile-status-inactive"
                                }
                            >

                                {profile?.isActive ? "Active" : "Inactive"}

                            </span>

                        }
                    />

                    <ViewItem
                        label="Created At"
                        value={formatDate(profile?.createdAt)}
                    />

                </DashboardSection>

            </div>

        </div>

    );

}