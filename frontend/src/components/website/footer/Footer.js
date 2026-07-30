"use client";

import Link from "next/link";
import Image from "next/image";
import "./Footer.css";

export default function Footer() {
    return (
        <footer className="footer">

            <div className="container">

                <div className="footer-top">

                    {/* Left */}

                    <div className="footer-about">

                        <Link href="/" className="footer-logo" 
                        onClick={() => window.scrollTo({ top: 0, behavior: "smooth" })}>

                            <Image
                                src="/images/logo/healtive-logo.png"
                                alt="Healtive"
                                width={190}
                                height={52}
                            />

                        </Link>

                        <p>

                            Simplifying hospital operations through one secure,
                            intelligent and cloud-based healthcare management
                            platform built for modern hospitals.

                        </p>

                    </div>

                    {/* Center */}

                    <div className="footer-links">

                        <h5>Company</h5>

                        <ul>

                            <li><Link href="/about">About Us</Link></li>

                            <li><Link href="/features">Features</Link></li>

                            <li><Link href="/pricing">Pricing</Link></li>

                            <li><Link href="/contact">Contact</Link></li>

                            <li><Link href="/privacy-policy">Privacy Policy</Link></li>

                            <li><Link href="/terms">Terms & Conditions</Link></li>

                        </ul>

                    </div>

                    {/* Right */}

                    <div className="footer-contact">

                        <h5>Get in touch</h5>

                        <a href="mailto:hello@healtive.com">
                            hello@healtive.com
                        </a>

                        <p>

                            Looking for a demo, partnership or support?
                            We'd love to hear from you.

                        </p>

                        <h6>Follow Us</h6>

                        <div className="footer-social">

                            <a href="#"><i className="bi bi-facebook"></i></a>

                            <a href="#"><i className="bi bi-instagram"></i></a>

                            <a href="#"><i className="bi bi-linkedin"></i></a>

                            <a href="#"><i className="bi bi-youtube"></i></a>

                            <a href="#"><i className="bi bi-twitter-x"></i></a>

                        </div>

                    </div>

                </div>

                <div className="footer-bottom">

                    <p>

                        © {new Date().getFullYear()} Healtive. All rights reserved.

                    </p>

                    <span>

                        Built with ❤️ for modern healthcare.

                    </span>

                </div>

            </div>

        </footer>
    );
}