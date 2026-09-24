"use client";

import Link from "next/link";
import { usePathname } from "next/navigation";

import "./PortalLayout.css";

export default function PortalSidebar({
    brand = "Healtive",
    roleLabel = "Doctor Portal",
    menuItems = [],
}) {

    const pathname = usePathname();

    return (

        <aside className="portal-sidebar">

            <div className="portal-sidebar-logo">

                <h3>{brand}</h3>

                <span>{roleLabel}</span>

            </div>

            <nav className="portal-sidebar-menu">

                {
                    menuItems.map((item) => {

                        const Icon = item.icon;

                        if (item.disabled) {

                            return (

                                <span
                                    key={item.title}
                                    className="portal-sidebar-link disabled"
                                    aria-disabled="true"
                                >

                                    <Icon size={20} />

                                    <span className="portal-sidebar-link-text">

                                        {item.title}

                                        <small>Soon</small>

                                    </span>

                                </span>

                            );

                        }

                        return (

                            <Link
                                key={item.href}
                                href={item.href}
                                className={
                                    pathname === item.href
                                        ? "portal-sidebar-link active"
                                        : "portal-sidebar-link"
                                }
                            >

                                <Icon size={20} />

                                <span>{item.title}</span>

                            </Link>

                        );

                    })
                }

            </nav>

        </aside>

    );

}