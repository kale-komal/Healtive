"use client";

import "./WhyHealtive.css";

const cards = [
  {
    icon: "bi-hospital",
    title: "Complete Hospital Management",
    text: "Manage OPD, IPD, appointments, doctors, pharmacy, laboratory, billing and inventory from one secure platform.",
    footer: "20+ Integrated Modules",
    blue: false,
  },
  {
    icon: "bi-shield-check",
    title: "Enterprise Security",
    text: "Role-based access, encrypted patient records, automatic backups and secure cloud infrastructure.",
    footer: "HIPAA Ready",
    blue: false,
  },
  {
    icon: "bi-lightning-charge",
    title: "Fast Cloud Performance",
    text: "Access your hospital from anywhere with lightning-fast cloud performance and 99.9% uptime.",
    footer: "99.9% Uptime",
    blue: true,
  },
  {
    icon: "bi-graph-up-arrow",
    title: "Smart Analytics",
    text: "Powerful dashboards help you monitor revenue, appointments, occupancy and hospital performance.",
    footer: "Live Reports",
    blue: false,
  },
  {
    icon: "bi-receipt",
    title: "Billing & Insurance",
    text: "Generate invoices, manage insurance claims and keep financial reports organized effortlessly.",
    footer: "Easy Accounting",
    blue: false,
  },
  {
    icon: "bi-headset",
    title: "Dedicated Support",
    text: "Implementation assistance, staff training and ongoing technical support whenever you need it.",
    footer: "24×7 Support",
    blue: false,
  },
];

export default function WhyHealtive() {
  return (
    <section className="why-healtive py-5">

      <div className="container">

        <div className="section-heading">

          <span>WHY HEALTIVE</span>

          <h2>
            Built for Modern Hospitals,
            <br />
            Designed for Better Patient Care.
          </h2>

          <p>
            Everything your hospital needs—from patient registration
            to billing and analytics—in one secure cloud platform.
          </p>

        </div>

        <div className="row g-4">

          {cards.map((card, index) => (

            <div className="col-lg-4 col-md-6" key={index}>

              <div className={`feature-card ${card.blue ? "blue-card" : ""}`}>

                <div className="feature-icon">
                  <i className={`bi ${card.icon}`}></i>
                </div>

                <h3>{card.title}</h3>

                <p>{card.text}</p>

                <div className="feature-footer">

                  {card.footer}

                </div>

              </div>

            </div>

          ))}

        </div>

      </div>

    </section>
  );
}