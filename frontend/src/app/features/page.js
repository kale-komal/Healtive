import Header from "@/components/website/header/Header";
import Footer from "@/components/website/footer/Footer";
import "./Features.css";

export default function FeaturesPage() {
    return (
        <>
            <Header />

            <main className="features-page">

                <div className="container">

                    <div className="row justify-content-center">

                        <div className="col-xl-10 col-lg-11">

                            {/* Hero */}

                            <section className="features-hero">

                                <span className="features-tag">
                                    Features
                                </span>

                                <h1>
                                    Everything your hospital
                                    <br />
                                    needs. One powerful platform.
                                </h1>

                                <p>
                                    From patient registration and appointments to
                                    pharmacy, laboratory, billing and analytics,
                                    Healtive connects every department through one
                                    secure cloud platform designed for modern
                                    healthcare organizations.
                                </p>

                                <div className="features-buttons">

                                    <a
                                        href="/get-started"
                                        className="btn-primary-custom"
                                    >
                                        Get Started
                                    </a>

                                    <a
                                        href="/pricing"
                                        className="btn-secondary-custom"
                                    >
                                        View Pricing
                                    </a>

                                </div>

                            </section>

                            {/* Feature Categories */}

<section className="feature-categories">

    <div className="section-heading">

        <span>Core Features</span>

        <h2>
            Everything you need to run
            <br />
            a modern hospital.
        </h2>

        <p>
            Healtive brings every department together into one connected
            platform, helping your team work faster, reduce errors and
            deliver better patient care.
        </p>

    </div>

    <div className="row g-4">

        <div className="col-lg-6">

            <div className="feature-box">

                <div className="feature-icon">

                    <i className="bi bi-people"></i>

                </div>

                <h3>Patient Management</h3>

                <p>
                    Register patients, maintain complete medical histories,
                    manage OPD/IPD records and access information instantly.
                </p>

            </div>

        </div>

        <div className="col-lg-6">

            <div className="feature-box">

                <div className="feature-icon">

                    <i className="bi bi-heart-pulse"></i>

                </div>

                <h3>Doctor Workspace</h3>

                <p>
                    Manage appointments, consultations, prescriptions,
                    follow-ups and patient notes from one dashboard.
                </p>

            </div>

        </div>

        <div className="col-lg-6">

            <div className="feature-box">

                <div className="feature-icon">

                    <i className="bi bi-receipt"></i>

                </div>

                <h3>Billing & Finance</h3>

                <p>
                    Generate invoices, process insurance claims, monitor
                    payments and keep financial records organized.
                </p>

            </div>

        </div>

        <div className="col-lg-6">

            <div className="feature-box">

                <div className="feature-icon">

                    <i className="bi bi-bar-chart"></i>

                </div>

                <h3>Reports & Analytics</h3>

                <p>
                    View real-time dashboards, operational insights and
                    performance reports to make informed decisions.
                </p>

            </div>

        </div>

    </div>

</section>

{/* Hospital Modules */}

<section className="hospital-modules">

    <div className="section-heading">

        <span>Complete Platform</span>

        <h2>
            Every module you need,
            <br />
            working together.
        </h2>

        <p>
            From front desk operations to pharmacy and analytics,
            every module is connected through one secure cloud platform.
        </p>

    </div>

    <div className="modules-box">

        <div className="row">

            <div className="col-lg-4">

                <ul className="module-list">

                    <li>Patient Registration</li>
                    <li>Appointment Scheduling</li>
                    <li>OPD Management</li>
                    <li>IPD Management</li>
                    <li>Doctor Management</li>
                    <li>Reception Dashboard</li>

                </ul>

            </div>

            <div className="col-lg-4">

                <ul className="module-list">

                    <li>Laboratory</li>
                    <li>Pharmacy</li>
                    <li>Billing & Insurance</li>
                    <li>Inventory</li>
                    <li>Staff Management</li>
                    <li>Role Permissions</li>

                </ul>

            </div>

            <div className="col-lg-4">

                <ul className="module-list">

                    <li>Analytics Dashboard</li>
                    <li>Reports</li>
                    <li>Multi Branch</li>
                    <li>Cloud Backup</li>
                    <li>SMS & Email</li>
                    <li>Audit Logs</li>

                </ul>

            </div>

        </div>

    </div>

</section>

{/* CTA */}

<section className="features-cta">

    <div className="features-cta-box">

        <span>
            Ready to get started?
        </span>

        <h2>
            Transform your hospital
            with one connected platform.
        </h2>

        <p>
            Join the next generation of healthcare providers using
            Healtive to simplify operations, improve patient care,
            and manage every department from one secure platform.
        </p>

        <div className="features-cta-buttons">

            <a href="#" className="btn-primary-custom">
                Get Started
            </a>

            <a href="#" className="btn-secondary-custom">
                Contact Sales
            </a>

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