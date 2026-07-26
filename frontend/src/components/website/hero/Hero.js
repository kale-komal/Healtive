"use client";

import Image from "next/image";
import "./Hero.css";

export default function Hero() {
    return (
        <section className="hero">

            <div className="container">

                <div className="row align-items-center">

                    {/* LEFT */}

                    <div className="col-lg-6">

                        <div className="hero-content">

                            <span className="hero-badge">

                                <i className="bi bi-stars"></i>

                                Modern Cloud Healthcare Platform

                            </span>

                            <h1 className="hero-title">

                                Manage Your

                                <br />

                                Hospital With

                                <br />

                                <span>Confidence.</span>

                            </h1>

                            <p className="hero-description">

                                Everything your hospital needs in one secure
                                platform—from patient registration and
                                appointments to billing, pharmacy, laboratory,
                                inventory and analytics.

                            </p>

                            <div className="hero-buttons">

                                <button className="btn-primary-custom">

                                    Start Free Trial

                                    <i className="bi bi-arrow-right"></i>

                                </button>

                                <button className="btn-secondary-custom">

                                    Book Demo

                                </button>

                            </div>

                            <div className="hero-stats">

                                <div>
                                    <h4>99.9%</h4>
                                    <span>Uptime</span>
                                </div>

                                <div>
                                    <h4>20+</h4>
                                    <span>Modules</span>
                                </div>

                                <div>
                                    <h4>24×7</h4>
                                    <span>Support</span>
                                </div>

                            </div>

                        </div>

                    </div>

                    {/* RIGHT */}

                    <div className="col-lg-6">

                        <div className="hero-image-wrapper">

                            <div className="hero-glow"></div>

                            <div className="hero-card">

                                <Image
                                    src="/images/home-banner.png"
                                    alt="Healtive Dashboard"
                                    width={700}
                                    height={550}
                                    priority
                                    className="hero-image"
                                />

                            </div>

                        </div>

                    </div>

                </div>

            </div>

        </section>
    );
}