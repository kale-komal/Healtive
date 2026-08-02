import Header from "@/components/website/header/Header";
import Footer from "@/components/website/footer/Footer";
import "./Privacy.css";

export default function PrivacyPage() {
    return (
        <>
            <Header />

            <main className="privacy-page">

                <div className="container">

                    <div className="row justify-content-center">

                        <div className="col-xl-10 col-lg-11">

                            {/* Hero */}

                            <section className="privacy-hero">

                                <span className="privacy-tag">
                                    Privacy Policy
                                </span>

                                <h1>
                                    Your privacy matters.
                                </h1>

                                <p>
                                    Learn how Healtive collects, uses and protects your
                                    information across our unified healthcare platform.
                                </p>

                                <span className="updated-date">
                                    Last Updated: June 17, 2026
                                </span>

                            </section>

                            {/* Content */}

                            <section className="privacy-content">

                                <p>
                                    At Healtive, we value the privacy of our users,
                                    particularly regarding protected health information
                                    and identity tracking data. This Privacy Policy
                                    outlines how we collect, use and protect information
                                    within our unified healthcare ecosystem.
                                </p>

                                <h2>
                                    1. Information We Collect
                                </h2>

                                <p>
                                    To operate the multi-role clinical workflow, we
                                    collect only the minimum information required based
                                    on your role.
                                </p>

                                <ul>

                                    <li>
                                        <strong>Patients:</strong> Full Name, Age,
                                        Gender, Known Allergies, Chronic Conditions
                                        and system-generated identification tokens.
                                    </li>

                                    <li>
                                        <strong>Healthcare Providers:</strong> Full
                                        Name, Professional Email, Clinic/Store Name
                                        and system configuration preferences.
                                    </li>

                                    <li>
                                        <strong>Usage Information:</strong> Device
                                        type, operating system, browser information
                                        and system event timestamps.
                                    </li>

                                </ul>

                                <h2>
                                    2. Use of Data & Content Security
                                </h2>

                                <p>
                                    Healtive processes information only for the
                                    healthcare workflow.
                                </p>

                                <ul>

                                    <li>
                                        Display live patient queue information.
                                    </li>

                                    <li>
                                        Securely display medical history during
                                        consultation.
                                    </li>

                                    <li>
                                        Share prescriptions only with authorized
                                        pharmacists after billing completion.
                                    </li>

                                </ul>

                                <p>
                                    <strong>No Third-Party Sharing:</strong> We never
                                    sell, rent or share patient information with
                                    advertisers or third-party data brokers.
                                </p>

                                <h2>
                                    3. Data Storage, Retention & Security
                                </h2>

                                <ul>

                                    <li>
                                        <strong>Database Security:</strong> All
                                        information is stored using encrypted
                                        databases.
                                    </li>

                                    <li>
                                        <strong>Data Destruction:</strong> Temporary
                                        operational tokens are permanently destroyed
                                        after completing the patient workflow.
                                    </li>

                                    <li>
                                        <strong>User Rights:</strong> Users may request
                                        deletion of their information by contacting
                                        Healtive support.
                                    </li>

                                </ul>

                                <h2>
                                    4. Compliance & Policy Updates
                                </h2>

                                <p>
                                    We may update this Privacy Policy to comply with
                                    applicable healthcare regulations and privacy
                                    laws. Any major updates will be announced within
                                    the Healtive platform.
                                </p>

                            </section>

                            {/* Bottom Link */}

                            <section className="privacy-bottom">

                                <a href="/terms">
                                    Read our Terms & Conditions →
                                </a>

                            </section>

                        </div>

                    </div>

                </div>

            </main>

            <Footer />
        </>
    );
}