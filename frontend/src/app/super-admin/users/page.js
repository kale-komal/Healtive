"use client";

import UserTable from "@/components/super-admin/user/UserTable";

export default function UsersPage() {

    return (

        <div className="container-fluid">

            <div className="page-header">

                <h2>
                    Users
                </h2>

                <p>
                    Manage and monitor system users.
                </p>

            </div>

            <UserTable />

        </div>

    );

}