import Header from "@/components/website/header/Header";
import Footer from "@/components/website/footer/Footer";
import "./Contact.css";

export default function ContactPage() {
    return (
        <>
            <Header />

            <main className="contact-page">

                <div className="container">

                    <div className="row justify-content-center">

                        <div className="col-xl-10 col-lg-11">

                            {/* Hero */}

                            <section className="contact-hero">

                                <span className="contact-tag">
                                    Contact
                                </span>

                                <h1>
                                    Let's start a
                                    conversation.
                                </h1>

                                <p>
                                    Have questions about Healtive, pricing,
                                    partnerships, or enterprise solutions?
                                    We'd love to hear from you.
                                </p>

                            </section>

                            {/* Contact Section */}

<section className="contact-section">

    <div className="row g-5">

        <div className="col-lg-7">

            <div className="contact-form-card">

                <h2>Send us a message</h2>

                <p>
                    Fill out the form below and our team will get back to
                    you as soon as possible.
                </p>

                <form>

                    <div className="row">

                        <div className="col-md-6 mb-4">

                            <label>Full Name</label>

                            <input
                                type="text"
                                className="form-control"
                                placeholder="John Doe"
                            />

                        </div>

                        <div className="col-md-6 mb-4">

                            <label>Email Address</label>

                            <input
                                type="email"
                                className="form-control"
                                placeholder="john@example.com"
                            />

                        </div>

                    </div>

                    <div className="mb-4">

                        <label>Subject</label>

                        <input
                            type="text"
                            className="form-control"
                            placeholder="How can we help?"
                        />

                    </div>

                    <div className="mb-4">

                        <label>Message</label>

                        <textarea
                            className="form-control"
                            rows="6"
                            placeholder="Write your message..."
                        ></textarea>

                    </div>

                    <button className="contact-btn">
                        Send Message
                    </button>

                </form>

            </div>

        </div>

        <div className="col-lg-5">

            <div className="contact-info">

                <h2>Contact Information</h2>

                <p>
                    Reach out to us through any of the following channels.
                </p>

                <div className="info-item">

                    <h6>Email</h6>

                    <span>healtive.care@gmail.com</span>

                </div>

                <div className="info-item">

                    <h6>Phone</h6>

                    <span>+91 98765 43210</span>

                </div>

                <div className="info-item">

                    <h6>Office</h6>

                    <span>
                        Navi Mumbai,
                        Maharashtra, India
                    </span>

                </div>

                <div className="info-item">

                    <h6>Working Hours</h6>

                    <span>
                        Monday – Friday
                        <br />
                        9:00 AM – 6:00 PM
                    </span>

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