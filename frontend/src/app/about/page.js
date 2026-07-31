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
</div>
</div>
</div>
</main>

<Footer />
    </>
  );
}