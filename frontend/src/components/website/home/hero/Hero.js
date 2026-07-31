"use client";

import Image from "next/image";
import "./Hero.css";

export default function Hero() {
    return (
        <section className="hero">

            <div className="hero-bg-shape hero-bg-left"></div>
            <div className="hero-bg-shape hero-bg-right"></div>

            <div className="container">

                <div className="row align-items-center gy-5">

                    {/* LEFT */}

                    <div className="col-lg-6">

                        <div className="hero-content">

                            <span className="hero-badge" data-aos="fade-down">
                                Trusted by Healthcare Organizations
                            </span>

                            <h1 className="hero-title" data-aos="fade-right" data-aos-delay="100">

                                Modern Hospital
                                <br />

                                Management

                                <br />

                                <span>Made Simple.</span>

                            </h1>

                            <p className="hero-description">

                                Manage patients, appointments, doctors,
                                pharmacy, laboratory, billing and inventory
                                from one secure cloud platform built for
                                modern hospitals.

                            </p>

                            <div className="hero-buttons">

                                <button className="btn-primary-custom">

                                    Start Free Trial

                                </button>

                                <button className="btn-secondary-custom">

                                    Book Demo

                                </button>

                            </div>

                            {/* <div className="hero-features">

                                <span>
                                    <i className="bi bi-check-circle-fill"></i>
                                    Cloud Based
                                </span>

                                <span>
                                    <i className="bi bi-check-circle-fill"></i>
                                    Multi Branch
                                </span>

                                <span>
                                    <i className="bi bi-check-circle-fill"></i>
                                    Secure
                                </span>

                                <span>
                                    <i className="bi bi-check-circle-fill"></i>
                                    24×7 Support
                                </span>

                            </div> */}

                        </div>

                    </div>

                    {/* RIGHT */}

                    <div className="col-lg-6">

                        <div className="hero-image-wrapper">

                            <div className="hero-image-card">

                                <Image
                                    src="/images/home-banner.png"
                                    alt="Healtive Dashboard"
                                    width={900}
                                    height={650}
                                    className="hero-image"
                                    priority
                                />

                            </div>

                        </div>

                    </div>

                </div>

            </div>

        </section>
    );
}