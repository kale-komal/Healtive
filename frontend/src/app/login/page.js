"use client";
import { useState } from "react";
import { useRouter } from "next/navigation";

import authService from "@/services/auth/authService";
import { useAuth } from "@/contexts/AuthContext";

import { toast } from "react-toastify";

import Link from "next/link";
import Header from "@/components/website/header/Header";
import Footer from "@/components/website/footer/Footer";
import "./Login.css";

export default function LoginPage() {

    const router = useRouter();

    const { login } = useAuth();

    const [loading, setLoading] = useState(false);

    const [formData, setFormData] = useState({
        usernameOrEmail: "",
        password: "",
    });

    const handleChange = (e) => {

        setFormData({
            ...formData,
            [e.target.name]: e.target.value,
        });

    };

    const handleLogin = async (e) => {

    e.preventDefault();

    if (!formData.usernameOrEmail.trim()) {
        toast.error("Username or Email is required.");
        return;
    }

    if (!formData.password.trim()) {
        toast.error("Password is required.");
        return;
    }

    try {

        setLoading(true);

        const response = await authService.login(formData);

        if (!response.success) {

            toast.error(response.message || "Login failed.");

            return;

        }

        login(response.data);

        toast.success(response.message);

        const role = response.data.user.role;

        switch (role) {

            case "SuperAdmin":

                router.push("/super-admin/dashboard");

                break;

            case "HospitalAdmin":

                router.push("/hospital-admin/dashboard");

                break;

            case "Doctor":

                router.push("/doctor/dashboard");

                break;

            case "Receptionist":

                router.push("/receptionist/dashboard");

                break;

            case "Pharmacist":

                router.push("/pharmacy/dashboard");

                break;

            default:

                router.push("/");

                break;

        }

    }
   catch (error) {

    console.log("Login Error:", error);

    console.log("Response:", error.response);

    console.log("Data:", error.response?.data);

    toast.error(
        error?.response?.data?.message ||
        error.message ||
        "Unable to login."
    );
}
    finally {

        setLoading(false);

    }

};

    return (

        <>

            <Header />

            <main className="login-page">

                <div className="container">

                    <div className="login-wrapper">

                        {/* Left Side */}

                        <div className="login-left">

                            <span className="login-tag">
                                Welcome Back
                            </span>

                            <h1>

                                Sign in to
                                <br />

                                <span>Healtive</span>

                            </h1>

                            <p>

                                Securely access your hospital dashboard,
                                manage appointments, patients, prescriptions
                                and daily workflows from one place.

                            </p>

                            <div className="login-info">

                                <div>

                                    <h4>Fast</h4>

                                    <p>
                                        Login in seconds and continue where you left off.
                                    </p>

                                </div>

                                <div>

                                    <h4>Secure</h4>

                                    <p>
                                        Enterprise-grade authentication with JWT security.
                                    </p>

                                </div>

                            </div>

                        </div>

                        {/* Right Side */}

                        <div className="login-card">

                            <h2>
                                Sign In
                            </h2>

                            <p>
                                Continue with your account
                            </p>

                            <form onSubmit={handleLogin}>

                                <div className="form-group">

                                    <label>
                                        Username or Email
                                    </label>

                                    <input
                                        type="text"
                                        name="usernameOrEmail"
                                        value={formData.usernameOrEmail}
                                        onChange={handleChange}
                                        placeholder="Enter username or email"
                                    />

                                </div>

                                <div className="form-group">

                                    <div className="password-head">

                                        <label>
                                            Password
                                        </label>

                                        <Link href="/forgot-password">
                                            Forgot Password?
                                        </Link>

                                    </div>

                                    <input
                                        type="password"
                                        name="password"
                                        value={formData.password}
                                        onChange={handleChange}
                                        placeholder="Enter password"
                                    />
                                </div>

                                <button
    className="login-btn"
    type="submit"
    disabled={loading}
>

    {
        loading
            ? "Signing In..."
            : "Continue with Email"
    }

</button>

                            </form>

                            <div className="login-divider">

                                <span>
                                    OR
                                </span>

                            </div>

                            <div className="signup-text">

                                Don't have an account?

                                <Link href="/register">

                                    Create Account

                                </Link>

                            </div>

                        </div>

                    </div>

                </div>

            </main>

            <Footer />

        </>

    );

}