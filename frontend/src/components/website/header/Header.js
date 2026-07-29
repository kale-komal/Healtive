"use client";

import Link from "next/link";
import Image from "next/image";
import "./Header.css";

export default function Header() {
    return (
        <header className="header">

            <div className="container">

                <nav className="header-inner">

                    {/* Logo */}

                    <Link href="/" className="logo">

                        <Image
                            src="/images/logo/healtive-logo.png"
                            alt="Healtive"
                            width={170}
                            height={48}
                            priority
                        />

                    </Link>

                    {/* Navigation */}

                    <ul className="nav-menu">

                        <li><Link href="/">Home</Link></li>

                        <li><Link href="/features">Features</Link></li>

                        <li><Link href="/modules">Modules</Link></li>

                        <li><Link href="/pricing">Pricing</Link></li>

                        <li><Link href="/about">About</Link></li>

                        <li><Link href="/contact">Contact</Link></li>

                    </ul>

                    {/* Right */}

                    <div className="header-actions">

                        <Link href="/login" className="btn-login">

                            Sign In

                        </Link>

                        <Link href="/login" className="btn-primary">

                            Get Started

                            <i className="bi bi-arrow-right"></i>

                        </Link>

                    </div>

                </nav>

            </div>

        </header>
    );
}