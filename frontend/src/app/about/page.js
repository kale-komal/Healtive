import Header from "@/components/website/header/Header";
import "./About.css";
import Footer from "@/components/website/footer/Footer";

export default function AboutPage() {
  return (
    <>

    <Header />
      <main className="about-page">

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
  Healtive replaces clipboards, whiteboards and disconnected
  tools with one sequential, secure workflow — so patients move
  smoothly from check-in to consultation to dispensing, and
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
                  <h3>Navi Mumbai</h3>
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
                  <h3>DPDPA Ready</h3>
                </div>
              </div>

            </div>

          </div>
        </section>

        

      </main>

      <Footer />
    </>
  );
}