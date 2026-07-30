"use client";

import Image from "next/image";
import "./Modules.css";

const modules = [
    {
        icon: "bi-person-vcard",
        title: "Patient Management",
        text: "Complete patient records"
    },
    {
        icon: "bi-calendar2-check",
        title: "Appointments",
        text: "Online & walk-in scheduling"
    },
    {
        icon: "bi-person-badge",
        title: "Doctor Management",
        text: "Doctors & departments"
    },
    {
        icon: "bi-capsule-pill",
        title: "Pharmacy",
        text: "Medicine inventory"
    },
    {
        icon: "bi-beaker",
        title: "Laboratory",
        text: "Tests & reports"
    },
    {
        icon: "bi-receipt",
        title: "Billing",
        text: "Invoices & insurance"
    }
];

export default function Modules() {

    return (

        <section className="modules-section">

            <div className="container">

                <div
                    className="section-heading"
                    data-aos="fade-up"
                >

                    <span data-aos="fade-up">HOSPITAL MODULES</span>

                    <h2 data-aos="fade-up" data-aos-delay="100">
                        Everything Your Hospital Needs,
                        <br />
                        All In One Platform.
                    </h2>

                    <p>
                        Powerful modules designed to simplify every
                        hospital operation—from patient registration to
                        billing, pharmacy and analytics.
                    </p>

                </div>

                <div className="row align-items-center gy-5">

                    {/* Dashboard */}

                    <div
                        className="col-lg-7"
                        data-aos="fade-right"
                    >

                        <div className="browser-card">

                            <div className="browser-header">

                                <span></span>

                                <span></span>

                                <span></span>

                            </div>

                            <div className="browser-body">

                                <Image
                                    src="/images/dashboard-preview.jpg"
                                    alt="Dashboard"
                                    width={1000}
                                    height={650}
                                    className="dashboard-image"
                                    priority
                                />

                            </div>

                        </div>

                    </div>

                    {/* Modules */}

                    <div
                        className="col-lg-5"
                    >

                        <div className="module-grid">

                            {modules.map((item, index) => (

                                <div
                                    className="module-card"
                                    key={index}
                                    data-aos="fade-left"
                                    data-aos-delay={index * 80}
                                >

                                    <div className="module-icon">

                                        <i className={`bi ${item.icon}`}></i>

                                    </div>

                                    <div>

                                        <h5>{item.title}</h5>

                                        <p>{item.text}</p>

                                    </div>

                                </div>

                            ))}

                        </div>

                        <div
                            className="module-footer"
                            data-aos="fade-up"
                        >

                            <span>20+ Integrated Modules</span>

                            <small>
                                Built for Hospitals • Clinics • Diagnostic Centers
                            </small>

                        </div>

                    </div>

                </div>

            </div>

        </section>

    );

}