"use client";

import Link from "next/link";
import Image from "next/image";
import "./Header.css";

export default function Header() {
    return (
        <header className="header">
            <div className="container">

                <nav className="navbar navbar-expand-lg header-wrapper">

                    {/* Logo */}

                    <Link href="/" className="navbar-brand logo">

                        <Image
                            src="/images/logo/healtive-logo.png"
                            alt="Healtive"
                            width={170}
                            height={48}
                            priority
                            className="logo-image"
                        />

                    </Link>

                    {/* Mobile Button */}

                    <button
                        className="navbar-toggler"
                        type="button"
                        data-bs-toggle="collapse"
                        data-bs-target="#mainNavbar"
                        aria-label="Toggle navigation"
                    >
                        <i className="bi bi-list"></i>
                    </button>

                    {/* Navigation */}

                    <div
                        className="collapse navbar-collapse"
                        id="mainNavbar"
                    >

                        <ul className="navbar-nav mx-auto">

                            <li className="nav-item">
                                <Link href="/" className="nav-link active">
                                    Home
                                </Link>
                            </li>

                            <li className="nav-item">
                                <Link href="/features" className="nav-link">
                                    Features
                                </Link>
                            </li>

                            <li className="nav-item">
                                <Link href="/modules" className="nav-link">
                                    Modules
                                </Link>
                            </li>

                            <li className="nav-item">
                                <Link href="/pricing" className="nav-link">
                                    Pricing
                                </Link>
                            </li>

                            <li className="nav-item">
                                <Link href="/about" className="nav-link">
                                    About
                                </Link>
                            </li>

                            <li className="nav-item">
                                <Link href="/contact" className="nav-link">
                                    Contact
                                </Link>
                            </li>

                        </ul>

                        <div className="header-buttons">

                            <Link href="/login" className="btn-signin">
                                Sign In
                            </Link>

                            <Link href="/login" className="btn-started">
                                Get Started
                                <i className="bi bi-arrow-right-short"></i>
                            </Link>

                        </div>

                    </div>

                </nav>

            </div>
        </header>
    );
}