import Header from "@/components/website/header/Header";
import Footer from "@/components/website/footer/Footer";
import "./Terms.css";

export const metadata = {
  title: "Terms & Conditions | Healtive",
  description:
    "The terms and conditions that govern your use of the Healtive platform and services.",
};

export default function TermsPage() {
  return (
    <>
      <Header />

      <main className="terms-page">

        <div className="container">

          <div className="row justify-content-center">

            <div className="col-xl-8 col-lg-9">

              <article className="terms-content">

                <h1>Terms & Conditions</h1>

                <p className="terms-date">
                  Last Updated: June 17, 2026
                </p>

                <p>
                  Please read these Terms and Conditions carefully before
                  using the Healtive platform, applications and services.
                  By accessing or using Healtive, you agree to comply with
                  these terms.
                </p>

                <h2>
                  1. Description of Service and Role-Based Accounts
                </h2>

                <p>
                  Healtive is a cloud-based healthcare operating system
                  connecting Patients, Receptionists, Doctors and
                  Pharmacists.
                </p>

                <ul>
                  <li>
                    Users must provide accurate registration information.
                  </li>

                  <li>
                    Users are responsible for safeguarding their account
                    credentials.
                  </li>

                  <li>
                    Users must provide their own devices and internet
                    connection.
                  </li>
                </ul>

                <h2>
                  2. Professional Medical Disclaimer
                </h2>

                <ul>
                  <li>
                    Healtive is a software platform and does not provide
                    medical advice or treatment.
                  </li>

                  <li>
                    Doctors remain fully responsible for prescriptions and
                    clinical decisions.
                  </li>

                  <li>
                    Pharmacists remain responsible for medication
                    verification and dispensing.
                  </li>
                </ul>

                <h2>
                  3. Financial Terms and Billing
                </h2>

                <ul>
                  <li>
                    Healtive does not process consultation payments.
                  </li>

                  <li>
                    Subscription fees are billed according to the selected
                    plan.
                  </li>

                  <li>
                    Subscription payments are non-refundable unless
                    otherwise stated.
                  </li>
                </ul>

                <h2>
                  4. QR Token Security
                </h2>

                <p>
                  Temporary prescription QR codes use single-use token
                  protection. Users must not duplicate or attempt to bypass
                  these security mechanisms.
                </p>

                <h2>
                  5. Limitation of Liability
                </h2>

                <p>
                  Healtive shall not be liable for indirect damages, loss
                  of data, loss of profits, or malpractice claims arising
                  from platform usage.
                </p>

                <div className="terms-link">

                  <a href="/privacy-policy">
                    Read our Privacy Policy →
                  </a>

                </div>

              </article>

            </div>

          </div>

        </div>

      </main>

      <Footer />
    </>
  );
}