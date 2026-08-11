"use client";

import { useEffect, useState } from "react";
import Link from "next/link";

import {
    Eye,
    Plus,
} from "lucide-react";

import { toast } from "react-toastify";

import userService from "@/services/user/userService";

import "./UserTable.css";

export default function UserTable() {

    const [users, setUsers] = useState([]);

    const [loading, setLoading] = useState(true);


    useEffect(() => {

        loadUsers();

    }, []);


    const loadUsers = async () => {

        try {

            setLoading(true);

            const response =
                await userService.getUsers();

            if (response.success) {

                setUsers(response.data || []);

            }
            else {

                toast.error(response.message);

            }

        }
        catch (error) {

            console.error("USER API ERROR:", error);

            console.error(
                "STATUS:",
                error.response?.status
            );

            console.error(
                "DATA:",
                error.response?.data
            );

            toast.error(
                error.response?.data?.message ||
                `Failed to load users. Status: ${error.response?.status || "Unknown"}`
            );

        }
        finally {

            setLoading(false);

        }

    };


    if (loading) {

        return (

            <div className="table-card">

                <div className="table-loading">

                    Loading users...

                </div>

            </div>

        );

    }


    return (

        <div className="table-card">

            {/* Header */}

            <div className="table-header">

                <h3>

                    Users

                </h3>

            </div>


            {/* Empty */}

            {users.length === 0 ? (

                <div className="empty-state">

                    No users found.

                </div>

            ) : (

                <div className="table-responsive">

                    <table className="table user-table">

                        <thead>

                            <tr>

                                <th>Name</th>

                                <th>Hospital</th>

                                <th>Role</th>

                                <th>Email</th>

                                <th>Mobile</th>

                                <th>Status</th>

                                <th>Created</th>

                                <th>Action</th>

                            </tr>

                        </thead>


                        <tbody>

                            {users.map((user) => (

                                <tr
                                    key={user.id}
                                >

                                    <td>

                                        <strong>

                                            {user.name}

                                        </strong>

                                    </td>


                                    <td>

                                        {user.hospitalName}

                                    </td>


                                    <td>

                                        {user.roleName}

                                    </td>


                                    <td>

                                        {user.email}

                                    </td>


                                    <td>

                                        {user.mobileNumber}

                                    </td>


                                    <td>

                                        <span
                                            className={
                                                user.isActive
                                                    ? "badge-active"
                                                    : "badge-inactive"
                                            }
                                        >

                                            {user.isActive
                                                ? "Active"
                                                : "Inactive"
                                            }

                                        </span>

                                    </td>


                                    <td>

                                        {new Date(
                                            user.createdAt
                                        ).toLocaleDateString()}

                                    </td>


                                    <td>

                                        <div className="action-buttons">

                                            <Link
                                                href={`/super-admin/users/view/${user.id}`}
                                                className="action-btn view"
                                                title="View"
                                            >

                                                <Eye
                                                    size={16}
                                                />

                                            </Link>

                                        </div>

                                    </td>

                                </tr>

                            ))}

                        </tbody>

                    </table>

                </div>

            )}

        </div>

    );

}