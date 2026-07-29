"use client";

import Image from "next/image";
import "./ConnectedData.css";

export default function ConnectedData() {
    return (

        <section className="connected-data">

            <div className="container">

                <div
                    className="connected-box"
                    data-aos="fade-up"
                >

                    <div className="row align-items-center gy-5">

                        {/* Left */}

                        <div
                            className="col-lg-6"
                            data-aos="fade-right"
                        >

                            <span className="section-tag">
                                One Platform
                            </span>

                            <h2>

                                All your healthcare data,

                                <br />

                                <span>connected in one place.</span>

                            </h2>

                            <p>

                                Healtive brings together patient records,
                                appointments, billing, laboratory, pharmacy,
                                inventory and administrative operations into
                                one secure cloud platform.

                            </p>

                        </div>

                        {/* Right */}

                        <div
                            className="col-lg-6 text-center d-flex align-item-center justify-content-center"
                            data-aos="fade-left"
                        >

                            <Image
                                src="/images/connected-data.png"
                                alt="Connected Data"
                                width={320}
                                height={320}
                                className="connected-image text-center"
                            />

                        </div>

                    </div>

                </div>

            </div>

        </section>

    );
}