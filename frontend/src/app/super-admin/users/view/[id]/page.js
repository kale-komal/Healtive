"use client";

import { useEffect, useState } from "react";
import { useParams, useRouter } from "next/navigation";
import { ArrowLeft } from "lucide-react";
import { toast } from "react-toastify";

import userService from "@/services/user/userService";

import "./UserView.css";

export default function UserViewPage() {

    const { id } = useParams();

    const router = useRouter();

    const [user, setUser] = useState(null);

    const [loading, setLoading] = useState(true);


    useEffect(() => {

        if (id) {
            loadUser();
        }

    }, [id]);


    const loadUser = async () => {

        try {

            const response =
                await userService.getUserById(id);

            if (response.success) {

                setUser(response.data);

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error("User View Error:", error);

            toast.error(
                "Failed to load user."
            );

        }
        finally {

            setLoading(false);

        }

    };


    if (loading) {

        return (

            <div className="view-card">

                <div className="view-loading">

                    Loading user...

                </div>

            </div>

        );

    }


    if (!user) {

        return (

            <div className="view-card">

                <div className="empty-state">

                    User not found.

                </div>

            </div>

        );

    }


    return (

        <div className="user-view-page">

            <div className="view-header">

                <div>

                    <h2>User Details</h2>

                    <p>
                        View user information
                    </p>

                </div>

                <button
                    className="btn btn-light"
                    onClick={() =>
                        router.push(
                            "/super-admin/users"
                        )
                    }
                >

                    <ArrowLeft size={17} />

                    Back

                </button>

            </div>


            <div className="view-card">

                <div className="view-section">

                    <h5>
                        Personal Information
                    </h5>

                    <div className="row">

                        <div className="col-md-6 mb-3">

                            <label>
                                First Name
                            </label>

                            <div className="view-value">
                                {user.firstName}
                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Last Name
                            </label>

                            <div className="view-value">
                                {user.lastName}
                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Email
                            </label>

                            <div className="view-value">
                                {user.email}
                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Mobile Number
                            </label>

                            <div className="view-value">
                                {user.mobileNumber}
                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Employee Code
                            </label>

                            <div className="view-value">
                                {user.employeeCode || "—"}
                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Role
                            </label>

                            <div className="view-value">

                                {user.roles?.length
                                    ? user.roles.join(", ")
                                    : "—"
                                }

                            </div>

                        </div>

                    </div>

                </div>


                <div className="view-section">

                    <h5>
                        Account Information
                    </h5>

                    <div className="row">

                        <div className="col-md-4 mb-3">

                            <label>
                                Status
                            </label>

                            <div>

                                <span
                                    className={
                                        user.isActive
                                            ? "badge-active"
                                            : "badge-inactive"
                                    }
                                >

                                    {user.isActive
                                        ? "Active"
                                        : "Inactive"
                                    }

                                </span>

                            </div>

                        </div>


                        <div className="col-md-4 mb-3">

                            <label>
                                Email Verified
                            </label>

                            <div>

                                <span
                                    className={
                                        user.isEmailVerified
                                            ? "badge-active"
                                            : "badge-inactive"
                                    }
                                >

                                    {user.isEmailVerified
                                        ? "Verified"
                                        : "Not Verified"
                                    }

                                </span>

                            </div>

                        </div>


                        <div className="col-md-4 mb-3">

                            <label>
                                Mobile Verified
                            </label>

                            <div>

                                <span
                                    className={
                                        user.isMobileVerified
                                            ? "badge-active"
                                            : "badge-inactive"
                                    }
                                >

                                    {user.isMobileVerified
                                        ? "Verified"
                                        : "Not Verified"
                                    }

                                </span>

                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Last Login
                            </label>

                            <div className="view-value">

                                {user.lastLoginAt
                                    ? new Date(
                                        user.lastLoginAt
                                    ).toLocaleString()
                                    : "Never"
                                }

                            </div>

                        </div>


                        <div className="col-md-6 mb-3">

                            <label>
                                Created
                            </label>

                            <div className="view-value">

                                {new Date(
                                    user.createdAt
                                ).toLocaleString()}

                            </div>

                        </div>

                    </div>

                </div>

            </div>

        </div>

    );

}