import Header from "@/components/website/header/Header";
import "./About.css";
import Footer from "@/components/website/footer/Footer";

export default function AboutPage() {
    return (
        <>

            <Header />

            <main className="about-page">

                <div className="container">

                    <div className="row justify-content-center">

                        <div className="col-xl-10 col-lg-11">

                            {/* Hero */}

                            <section className="about-hero">

                                <div className="container">

                                    <span className="about-tag">
                                        About Healtive
                                    </span>

                                    <h1>
                                        We're building the calm operating
                                        system for modern clinics.
                                    </h1>

                                    <p>
                                        Healtive replaces clipboards, whiteboards and
                                        disconnected tools with one sequential, secure
                                        workflow — so patients move smoothly from
                                        check-in to consultation to dispensing, and
                                        clinic teams finally get their day back.
                                    </p>



                                </div>

                            </section>

                            {/* Stats */}

                            <section className="about-stats">

                                <div className="container">

                                    <div className="row g-4">

                                        <div className="col-lg-3 col-md-6">
                                            <div className="stat-card">
                                                <span>Founded</span>
                                                <h3>2026</h3>
                                            </div>
                                        </div>

                                        <div className="col-lg-3 col-md-6">
                                            <div className="stat-card">
                                                <span>Headquarters</span>
                                                <h3>Navi Mumbai, India</h3>
                                            </div>
                                        </div>

                                        <div className="col-lg-3 col-md-6">
                                            <div className="stat-card">
                                                <span>Focus</span>
                                                <h3>Healthcare OS</h3>
                                            </div>
                                        </div>

                                        <div className="col-lg-3 col-md-6">
                                            <div className="stat-card">
                                                <span>Compliance</span>
                                                <h3>DPDPA-Architecture</h3>
                                            </div>
                                        </div>

                                    </div>

                                </div>

                            </section>

                            {/* Mission */}

                            <section className="about-mission">

                                <div className="row g-4">

                                    <div className="col-lg-4">

                                        <div className="mission-card">

                                            <div className="mission-icon">
                                                <i className="bi bi-heart-pulse"></i>
                                            </div>

                                            <h3>Our mission</h3>

                                            <p>
                                                Make every clinic visit feel calm,
                                                predictable and dignified — for patients
                                                and for the teams who care for them.
                                            </p>

                                        </div>

                                    </div>

                                    <div className="col-lg-4">

                                        <div className="mission-card">

                                            <div className="mission-icon">
                                                <i className="bi bi-bounding-box-circles"></i>
                                            </div>

                                            <h3>Our approach</h3>

                                            <p>
                                                One sequential workflow across
                                                reception, consultation and pharmacy.
                                                No duplicate entry, no shouting names
                                                across waiting rooms.
                                            </p>

                                        </div>

                                    </div>

                                    <div className="col-lg-4">

                                        <div className="mission-card">

                                            <div className="mission-icon">
                                                <i className="bi bi-shield-check"></i>
                                            </div>

                                            <h3>Our promise</h3>

                                            <p>
                                                Patient data stays encrypted,
                                                access stays role-scoped,
                                                and the system stays
                                                out of your way.
                                            </p>

                                        </div>

                                    </div>

                                </div>

                            </section>

                            {/* Story */}

                            <section className="about-story">

                                <div className="story-card">

                                    <h2>Our story</h2>

                                    <p>
                                        Healtive started after watching small and mid-sized clinics juggle five different tools to do one job: see a patient. Paper tokens, WhatsApp messages, spreadsheets, billing software and pharmacy registers — none of them talking to each other.
                                    </p>

                                    <p>
                                        We set out to build a single, opinionated workflow engine: a patient checks in via QR, the receptionist assigns them to the next available doctor, the doctor consults and prescribes, and the pharmacist dispenses — all in one continuous, auditable flow.
                                    </p>

                                    <p>
                                        Today, Healtive powers clinics that care about speed, accuracy and patient experience — without asking their staff to become software engineers.
                                    </p>

                                </div>

                            </section>
                            {/* What We Build */}

                            <section className="about-build">

                                <h2 className="section-title">
                                    What we build
                                </h2>

                                <div className="row g-4">

                                    <div className="col-lg-6">

                                        <div className="build-card">

                                            <div className="build-icon">
                                                <i className="bi bi-qr-code-scan"></i>
                                            </div>

                                            <div>

                                                <h4>QR-native check-in</h4>

                                                <p>
                                                    Patients scan, register and join the queue
                                                    in under a minute — no app install required.
                                                </p>

                                            </div>

                                        </div>

                                    </div>

                                    <div className="col-lg-6">

                                        <div className="build-card">

                                            <div className="build-icon">
                                                <i className="bi bi-bounding-box-circles"></i>
                                            </div>

                                            <div>

                                                <h4>Sequential workflow engine</h4>

                                                <p>
                                                    Reception → Doctor → Pharmacy,
                                                    with live status visible to everyone who needs it.
                                                </p>

                                            </div>

                                        </div>

                                    </div>

                                    <div className="col-lg-6">

                                        <div className="build-card">

                                            <div className="build-icon">
                                                <i className="bi bi-lock"></i>
                                            </div>

                                            <div>

                                                <h4>Zero-leak dispensing</h4>

                                                <p>
                                                    Prescriptions flow directly to the pharmacist
                                                    with stock checks and audit trails built in.
                                                </p>

                                            </div>

                                        </div>

                                    </div>

                                    <div className="col-lg-6">

                                        <div className="build-card">

                                            <div className="build-icon">
                                                <i className="bi bi-stars"></i>
                                            </div>

                                            <div>

                                                <h4>Calm, minimalist UX</h4>

                                                <p>
                                                    Designed for staff under pressure —
                                                    fewer clicks, clearer screens,
                                                    faster decisions.
                                                </p>

                                            </div>

                                        </div>

                                    </div>

                                </div>

                            </section>


                            {/* What We Value */}

                            <section className="about-values">

                                <h2 className="section-title">
                                    What we value
                                </h2>

                                <div className="row g-4">

                                    <div className="col-lg-4">

                                        <div className="value-card">

                                            <h4>Patients first</h4>

                                            <p>
                                                Every design decision is judged by whether
                                                it makes the patient's visit calmer.
                                            </p>

                                        </div>

                                    </div>

                                    <div className="col-lg-4">

                                        <div className="value-card">

                                            <h4>Privacy by default</h4>

                                            <p>
                                                End-to-end encryption,
                                                role-based access,
                                                and DPDPA-Architecture
                                                storage are non-negotiable.
                                            </p>

                                        </div>

                                    </div>

                                    <div className="col-lg-4">

                                        <div className="value-card">

                                            <h4>Boringly reliable</h4>

                                            <p>
                                                Healthcare doesn't need flashy.
                                                It needs systems that just work,
                                                every single day.
                                            </p>

                                        </div>

                                    </div>

                                </div>

                            </section>
                            {/* Contact CTA */}

                            <section className="about-contact">

                                <div className="contact-box">

                                    <div className="row align-items-center">

                                        <div className="col-lg-7">

                                            <h2>Want to talk to us?</h2>

                                            <p>
                                                Partnerships, demos, or support — we usually reply
                                                within 24 hours.
                                            </p>

                                            <div className="contact-social">

                                                <a href="#">
                                                    <i className="bi bi-envelope"></i>
                                                </a>

                                                <a href="https://www.instagram.com/healtive.care" target="blank">
                                                    <i className="bi bi-instagram"></i>
                                                </a>

                                                <a href="https://in.linkedin.com/company/healtive" target="blank">
                                                    <i className="bi bi-linkedin"></i>
                                                </a>

                                            </div>

                                        </div>

                                        <div className="col-lg-5 text-lg-end mt-4 mt-lg-0">

                                            <a
                                                href="mailto:healtive.care@gmail.com"
                                                className="contact-btn"
                                            >
                                                healtive.care@gmail.com
                                            </a>

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