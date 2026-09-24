"use client";

import PortalSidebar from "./PortalSidebar";
import PortalHeader from "./PortalHeader";

import "./PortalLayout.css";

export default function PortalLayout({
    children,
    brand = "Healtive",
    roleLabel = "Doctor Portal",
    menuItems = [],
}) {

    return (

        <div className="portal-layout">

            <PortalSidebar
                brand={brand}
                roleLabel={roleLabel}
                menuItems={menuItems}
            />

            <div className="portal-main">

                <PortalHeader />

                <div className="portal-content">

                    {children}

                </div>

            </div>

        </div>

    );

}