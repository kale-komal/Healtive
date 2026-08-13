"use client";

import { useState } from "react";
import { Lock } from "lucide-react";
import { toast } from "react-toastify";

import userService from "@/services/user/userService";

import "./ChangePasswordForm.css";

export default function ChangePasswordForm() {

    const [formData, setFormData] = useState({
        currentPassword: "",
        newPassword: "",
        confirmPassword: "",
    });

    const [loading, setLoading] = useState(false);


    const handleChange = (e) => {

        const { name, value } = e.target;

        setFormData((prev) => ({
            ...prev,
            [name]: value,
        }));

    };


    const handleSubmit = async (e) => {

        e.preventDefault();


        if (!formData.currentPassword) {

            toast.error("Current password is required.");

            return;

        }


        if (!formData.newPassword) {

            toast.error("New password is required.");

            return;

        }


        if (formData.newPassword.length < 6) {

            toast.error(
                "New password must be at least 6 characters."
            );

            return;

        }


        if (!formData.confirmPassword) {

            toast.error("Please confirm your new password.");

            return;

        }


        if (
            formData.newPassword !==
            formData.confirmPassword
        ) {

            toast.error(
                "New password and confirm password do not match."
            );

            return;

        }


        if (
            formData.currentPassword ===
            formData.newPassword
        ) {

            toast.error(
                "New password must be different from current password."
            );

            return;

        }


        try {

            setLoading(true);

            const response =
                await userService.changePassword(formData);


            if (response.success) {

                toast.success(
                    response.message ||
                    "Password changed successfully."
                );


                setFormData({
                    currentPassword: "",
                    newPassword: "",
                    confirmPassword: "",
                });

            }
            else {

                toast.error(
                    response.message ||
                    "Failed to change password."
                );

            }

        }
        catch (error) {

            console.error(
                "Change Password Error:",
                error
            );

            const message =
                error.response?.data?.message ||
                "Something went wrong.";

            toast.error(message);

        }
        finally {

            setLoading(false);

        }

    };


    return (

        <div className="change-password-card">

            <div className="change-password-header">

                <div className="change-password-icon">

                    <Lock size={22} />

                </div>

                <div>

                    <h3>Change Password</h3>

                    <p>
                        Update your account password.
                    </p>

                </div>

            </div>


            <form
                onSubmit={handleSubmit}
                className="change-password-form"
            >

                <div className="form-group">

                    <label>
                        Current Password
                    </label>

                    <input
                        type="password"
                        name="currentPassword"
                        value={formData.currentPassword}
                        onChange={handleChange}
                        placeholder="Enter current password"
                        disabled={loading}
                    />

                </div>


                <div className="form-group">

                    <label>
                        New Password
                    </label>

                    <input
                        type="password"
                        name="newPassword"
                        value={formData.newPassword}
                        onChange={handleChange}
                        placeholder="Enter new password"
                        disabled={loading}
                    />

                </div>


                <div className="form-group">

                    <label>
                        Confirm New Password
                    </label>

                    <input
                        type="password"
                        name="confirmPassword"
                        value={formData.confirmPassword}
                        onChange={handleChange}
                        placeholder="Confirm new password"
                        disabled={loading}
                    />

                </div>


                <button
                    type="submit"
                    className="btn btn-primary"
                    disabled={loading}
                >

                    {loading
                        ? "Changing Password..."
                        : "Change Password"
                    }

                </button>

            </form>

        </div>

    );

}