"use client";

import { useState } from "react";
import Header from "@/components/website/header/Header";
import Footer from "@/components/website/footer/Footer";
import "./Pricing.css";

export default function PricingPage() {

    const [billing, setBilling] = useState("monthly");

    return (
        <>
            <Header />

            <main className="pricing-page">

                <div className="container">

                    <div className="row justify-content-center">

                        <div className="">

                            {/* Hero */}

                            <section className="pricing-hero">

                                <h1>
                                    Simple, transparent pricing
                                    <br />
                                    <span>for every clinic.</span>
                                </h1>

                                <p>
                                    Start building your digital health infrastructure today.
                                    Our Team on all paid plans.
                                </p>

                                <div className="billing-toggle">

                                    <button
                                        className={billing === "monthly" ? "active" : ""}
                                        onClick={() => setBilling("monthly")}
                                    >
                                        Monthly
                                    </button>

                                    <button
                                        className={billing === "annually" ? "active" : ""}
                                        onClick={() => setBilling("annually")}
                                    >
                                        Annually

                                        <span>Save 10%</span>

                                    </button>

                                </div>

                            </section>

                            {/* Plans */}

                            <section className="pricing-plans">

                                <div className="row g-4 align-items-stretch">

                                    {/* Lite */}

                                    <div className="col-lg-4">

                                        <div className="pricing-card">

                                            <h5>Lite</h5>

                                            <div className="price">

                                                {billing === "monthly" ? "₹4,999" : "₹4,499"}

                                                <small>/month</small>

                                                {billing === "annually" && <del>₹4,999</del>}

                                            </div>

                                            {billing === "annually" && (

                                                <p className="bill-info">
                                                    Billed annually — 10% off
                                                </p>

                                            )}



                                            <ul>

                                                <li>Basic patient records</li>

                                                <li>Secure QR check-in</li>

                                                <li>Single practitioner support</li>

                                                <li>24/7 support</li>

                                            </ul>

                                            <a href="#" className="pricing-btn">
                                                Start Free Trial
                                            </a>

                                        </div>

                                    </div>

                                    {/* Premium */}

                                    <div className="col-lg-4">

                                        <div className="pricing-card active">

                                            <span className="recommended">
                                                RECOMMENDED
                                            </span>

                                            <h5>Premium</h5>

                                            <div className="price">

                                                {billing === "monthly" ? "₹14,999" : "₹13,499"}

                                                <small>/month</small>

                                                {billing === "annually" && <del>₹14,999</del>}

                                            </div>

                                            {billing === "annually" && (

                                                <p className="bill-info">
                                                    Billed annually — 10% off
                                                </p>

                                            )}


                                            <ul>

                                                <li>Everything in Lite</li>

                                                <li>Multi-clinic management</li>

                                                <li>Advanced analytics</li>

                                                <li>Zero-leak prescriptions</li>

                                                <li>Priority integration support</li>

                                            </ul>

                                            <a href="#" className="pricing-btn white">
                                                Start Free Trial
                                            </a>

                                        </div>

                                    </div>

                                    {/* Enterprise */}

                                    <div className="col-lg-4">

                                        <div className="pricing-card">

                                            <h5>Enterprise</h5>

                                            <div className="price custom">
                                                Custom Pricing
                                            </div>

                                            <p className="enterprise-text">
                                                Tailored solutions for large health systems
                                                and hospital networks.
                                            </p>

                                            <ul>

                                                <li>Everything in Premium</li>

                                                <li>Unlimited practitioners</li>

                                                <li>Custom API endpoints</li>

                                                <li>Dedicated account manager</li>

                                                <li>White-labeling</li>

                                            </ul>

                                            <a href="#" className="pricing-btn">
                                                Contact Us
                                            </a>

                                        </div>

                                    </div>

                                </div>

                            </section>

                            {/* Bottom Note */}

                            <div className="pricing-note">

                                All plans include a 1-week free trial.
                                No hidden fees.

                            </div>

                            {/* FAQ */}

                            <section className="pricing-faq">
                                <div className="row justify-content-center">
                                    <div className="col-md-10">

                                        <h2>Frequently asked questions</h2>

                                        <div className="accordion" id="pricingFaq">

                                            <div className="accordion-item">

                                                <h2 className="accordion-header">

                                                    <button
                                                        className="accordion-button collapsed"
                                                        type="button"
                                                        data-bs-toggle="collapse"
                                                        data-bs-target="#faq1"
                                                        aria-expanded="false"
                                                    >
                                                        Can I switch plans later?
                                                    </button>

                                                </h2>

                                                <div
                                                    id="faq1"
                                                    className="accordion-collapse collapse"
                                                    data-bs-parent="#pricingFaq"
                                                >

                                                    <div className="accordion-body">
                                                        Yes, you can upgrade or downgrade your plan at any time. Changes are applied immediately and pro-rated on your next billing cycle.                                            </div>

                                                </div>

                                            </div>

                                            <div className="accordion-item">

                                                <h2 className="accordion-header">

                                                    <button
                                                        className="accordion-button collapsed"
                                                        type="button"
                                                        data-bs-toggle="collapse"
                                                        data-bs-target="#faq2"
                                                        aria-expanded="false"
                                                    >
                                                        Is patient data secure?
                                                    </button>

                                                </h2>

                                                <div
                                                    id="faq2"
                                                    className="accordion-collapse collapse"
                                                    data-bs-parent="#pricingFaq"
                                                >

                                                    <div className="accordion-body">
                                                        Security is our top priority. All Healtive plans include end-to-end encryption and are fully compliant with local data protection laws.                                            </div>

                                                </div>

                                            </div>

                                        </div>
                                    </div>
                                </div>

                            </section>

                        </div>

                    </div>

                </div>

            </main>

            <Footer />
        </>
    );
}