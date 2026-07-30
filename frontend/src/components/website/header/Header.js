"use client";

import { useState, useEffect } from "react";
import Link from "next/link";
import Image from "next/image";
import "./Header.css";

export default function Header() {

    const [menuOpen, setMenuOpen] = useState(false);

    useEffect(() => {
        document.body.style.overflow = menuOpen ? "hidden" : "";

        return () => {
            document.body.style.overflow = "";
        };
    }, [menuOpen]);

    const closeMenu = () => {
        setMenuOpen(false);
    };

    return (
        <>

            {/* ================= HEADER ================= */}

            <header className="header">

                <div className="container">

                    <nav className="header-inner">

                        {/* Logo */}

                        <Link
                            href="/"
                            className="logo"
                            onClick={closeMenu}
                        >
                            <Image
                                src="/images/logo/healtive-logo.png"
                                alt="Healtive"
                                width={170}
                                height={48}
                                priority
                            />
                        </Link>

                        {/* Desktop Navigation */}

                        <ul className="nav-menu">

                            <li><Link href="/">Home</Link></li>

                            <li><Link href="/features">Features</Link></li>

                            <li><Link href="/modules">Modules</Link></li>

                            <li><Link href="/pricing">Pricing</Link></li>

                            <li><Link href="/about">About</Link></li>

                            <li><Link href="/contact">Contact</Link></li>

                        </ul>

                        {/* Desktop Buttons */}

                        <div className="header-actions">

                            <Link href="/login" className="btn-login">

                                Sign In

                            </Link>

                            <Link href="/login" className="btn-started">

                                Get Started

                                <i className="bi bi-arrow-right"></i>

                            </Link>

                        </div>

                        {/* Mobile Toggle */}

                        <button
                            className="mobile-toggle"
                            onClick={() => setMenuOpen(true)}
                            aria-label="Open Menu"
                        >

                            <i className="bi bi-list"></i>

                        </button>

                    </nav>

                </div>

            </header>

            {/* ================= OVERLAY ================= */}

            <div
                className={`mobile-overlay ${menuOpen ? "show" : ""}`}
                onClick={closeMenu}
            ></div>

            {/* ================= SIDEBAR ================= */}

            <aside className={`mobile-sidebar ${menuOpen ? "show" : ""}`}>

                {/* Close */}

                <button
                    className="close-menu"
                    onClick={closeMenu}
                    aria-label="Close Menu"
                >

                    <i className="bi bi-x-lg"></i>

                </button>

                {/* Navigation */}

                <ul>

                    <li>
                        <Link href="/" onClick={closeMenu}>
                            Home
                        </Link>
                    </li>

                    <li>
                        <Link href="/features" onClick={closeMenu}>
                            Features
                        </Link>
                    </li>

                    <li>
                        <Link href="/modules" onClick={closeMenu}>
                            Modules
                        </Link>
                    </li>

                    <li>
                        <Link href="/pricing" onClick={closeMenu}>
                            Pricing
                        </Link>
                    </li>

                    <li>
                        <Link href="/about" onClick={closeMenu}>
                            About
                        </Link>
                    </li>

                    <li>
                        <Link href="/contact" onClick={closeMenu}>
                            Contact
                        </Link>
                    </li>

                </ul>

                {/* Bottom Buttons */}

                <div className="mobile-buttons">

                    <Link
                        href="/login"
                        className="btn-login"
                        onClick={closeMenu}
                    >
                        Sign In
                    </Link>

                    <Link
                        href="/login"
                        className="btn-started"
                        onClick={closeMenu}
                    >
                        Get Started

                        <i className="bi bi-arrow-right"></i>

                    </Link>

                </div>

            </aside>

        </>
    );
}