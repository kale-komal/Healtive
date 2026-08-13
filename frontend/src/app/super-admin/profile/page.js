"use client";

import { useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import { ArrowLeft, LockKeyhole } from "lucide-react";
import { toast } from "react-toastify";

import userService from "@/services/user/userService";

import "./Profile.css";

export default function ProfilePage() {

    const router = useRouter();

    const [profile, setProfile] = useState(null);

    const [loading, setLoading] = useState(true);


    useEffect(() => {

        loadProfile();

    }, []);


   const loadProfile = async () => {

    try {

        const response =
            await userService.getProfile();

        console.log("PROFILE API RESPONSE:", response);

        if (response.success) {

            console.log("PROFILE DATA:", response.data);

            setProfile(response.data);

        }
        else {

            console.log(
                "PROFILE API FAILED:",
                response.message
            );

            toast.error(response.message);

        }

    }
    catch (error) {

        console.error(
            "PROFILE API ERROR:",
            error
        );

        console.error(
            "ERROR RESPONSE:",
            error.response?.data
        );

        toast.error(
            "Failed to load profile."
        );

    }
    finally {

        setLoading(false);

    }

};


    if (loading) {

        return (

            <div className="profile-card">

                <div className="profile-loading">
                    Loading profile...
                </div>

            </div>

        );

    }


    if (!profile) {

        return (

            <div className="profile-card">

                <div className="profile-empty">
                    Profile not found.
                </div>

            </div>

        );

    }


    return (

        <div className="profile-page">

            {/* Header */}

            <div className="profile-header">

                <div>

                    <h2>
                        My Profile
                    </h2>

                    <p>
                        Manage your account information
                    </p>

                </div>

                <button
                    type="button"
                    className="btn btn-light"
                    onClick={() =>
                        router.push(
                            "/super-admin/dashboard"
                        )
                    }
                >

                    <ArrowLeft size={17} />

                    Back

                </button>

            </div>


            {/* Profile Card */}

            <div className="profile-card">

                {/* Profile Header */}

                <div className="profile-summary">

                    <div className="profile-avatar">

                        {profile.profileImageUrl ? (

                            <img
                                src={
                                    profile.profileImageUrl
                                }
                                alt="Profile"
                            />

                        ) : (

                            <span>

                                {profile.firstName
                                    ?.charAt(0)
                                    ?.toUpperCase()
                                }

                            </span>

                        )}

                    </div>


                    <div>

                        <h3>

                            {profile.firstName}{" "}
                            {profile.lastName}

                        </h3>

                        <p>

                            {profile.roles?.length
                                ? profile.roles.join(", ")
                                : "User"
                            }

                        </p>

                        <span
                            className={
                                profile.isActive
                                    ? "badge-active"
                                    : "badge-inactive"
                            }
                        >

                            {profile.isActive
                                ? "Active"
                                : "Inactive"
                            }

                        </span>

                    </div>

                </div>


                {/* Personal Information */}

                <div className="profile-section">

                    <h5>
                        Personal Information
                    </h5>

                    <div className="row">

                        <div className="col-md-6 mb-3">

                            <label>
                                First Name
                            </label>

                            <div className="profile-value">
                                {profile.firstName}
                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Last Name
                            </label>

                            <div className="profile-value">
                                {profile.lastName}
                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Email
                            </label>

                            <div className="profile-value">
                                {profile.email}
                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Mobile Number
                            </label>

                            <div className="profile-value">
                                {profile.mobileNumber}
                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Employee Code
                            </label>

                            <div className="profile-value">

                                {profile.employeeCode ||
                                    "—"
                                }

                            </div>

                        </div>

                    </div>

                </div>


                {/* Account Information */}

                <div className="profile-section">

                    <h5>
                        Account Information
                    </h5>

                    <div className="row">

                        <div className="col-md-4 mb-3">

                            <label>
                                Role
                            </label>

                            <div className="profile-value">

                                {profile.roles?.length
                                    ? profile.roles.join(", ")
                                    : "—"
                                }

                            </div>

                        </div>


                        <div className="col-md-4 mb-3">

                            <label>
                                Email Verification
                            </label>

                            <span
                                className={
                                    profile.isEmailVerified
                                        ? "badge-active"
                                        : "badge-inactive"
                                }
                            >

                                {profile.isEmailVerified
                                    ? "Verified"
                                    : "Not Verified"
                                }

                            </span>

                        </div>


                        <div className="col-md-4 mb-3">

                            <label>
                                Mobile Verification
                            </label>

                            <span
                                className={
                                    profile.isMobileVerified
                                        ? "badge-active"
                                        : "badge-inactive"
                                }
                            >

                                {profile.isMobileVerified
                                    ? "Verified"
                                    : "Not Verified"
                                }

                            </span>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Last Login
                            </label>

                            <div className="profile-value">

                                {profile.lastLoginAt
                                    ? new Date(
                                        profile.lastLoginAt
                                    ).toLocaleString()
                                    : "Never"
                                }

                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Account Created
                            </label>

                            <div className="profile-value">

                                {new Date(
                                    profile.createdAt
                                ).toLocaleString()}

                            </div>

                        </div>

                    </div>

                </div>


                {/* Security */}

                <div className="profile-section security-section">

                    <div>

                        <h5>
                            Security
                        </h5>

                        <p>
                            Update your account password
                            regularly to keep your account secure.
                        </p>

                    </div>

                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={() =>
                            router.push(
                                "/super-admin/settings"
                            )
                        }
                    >

                        <LockKeyhole size={17} />

                        Change Password

                    </button>

                </div>

            </div>

        </div>

    );

}