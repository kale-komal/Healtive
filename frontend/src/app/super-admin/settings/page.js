"use client";

import ChangePasswordForm from "@/components/auth/ChangePasswordForm";



export default function SettingsPage() {

    return (

        <div className="admin-page">

            <div className="page-header">

                <h1>Settings</h1>

                <p>
                    Manage your account settings.
                </p>

            </div>

            <ChangePasswordForm />

        </div>

    );

}