"use client";

import { useCallback, useEffect, useState } from "react";

import {
    Plus,
    Pencil,
    Eye,
    Power,
    Trash2,
    Users,
    X,
    Save,
    Loader2,
    ChevronDown,
} from "lucide-react";

import { toast } from "react-toastify";
import Swal from "sweetalert2";

import hospitalAdminService from "@/services/hospital-admin/hospitalAdminService";
import PageHeader from "@/components/super-admin/PageHeader";
import Pagination from "@/components/common/Pagination";
import {
    generateCodeFromName,
    uniqueCode,
} from "@/utils/codeGenerator";

import "./Staff.css";

const NON_STAFF_ROLES = new Set([
    "SuperAdmin",
    "HospitalAdmin",
    "Doctor",
]);

const normalizeResponse = (data) => {

    // Backend returns the { success, message, data, errors } envelope.
    if (
        data &&
        typeof data === "object" &&
        !Array.isArray(data) &&
        "success" in data
    ) {

        return {
            ok: !!data.success,
            message: data.message,
            data: data.success ? data.data : null,
        };

    }

    return {
        ok: data !== null && data !== undefined,
        message: "",
        data,
    };

};

const escapeHtml = (value) =>
    String(value ?? "")
        .replace(/&/g, "&amp;")
        .replace(/</g, "&lt;")
        .replace(/>/g, "&gt;")
        .replace(/"/g, "&quot;");

const formatDate = (value) => {

    if (!value) return "—";

    const date = new Date(value);

    if (Number.isNaN(date.getTime())) return "—";

    return date.toLocaleDateString("en-IN", {
        day: "2-digit",
        month: "short",
        year: "numeric",
    });

};

const isValidEmail = (value) =>
    /^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(value.trim());

const isValidPhone = (value) => {

    const digits = (value || "").replace(/[^\d]/g, "");

    return digits.length >= 10 && digits.length <= 15;

};

const suggestUsername = (firstName, lastName) => {

    const first = (firstName || "")
        .trim()
        .toLowerCase()
        .replace(/\s+/g, "");

    const last = (lastName || "")
        .trim()
        .toLowerCase()
        .replace(/\s+/g, "");

    if (!first && !last) return "";

    if (first && last) return `${first}.${last}`;

    return first || last;

};

const toStaffForm = (staff) => ({

    employeeCode: staff?.employeeCode || "",
    username: staff?.username || "",
    firstName: staff?.firstName || "",
    lastName: staff?.lastName || "",
    email: staff?.email || "",
    mobileNumber: staff?.mobileNumber || "",
    roleId: staff?.roleId || "",

});

const buildCreatePayload = (form) => ({

    employeeCode: form.employeeCode.trim(),
    username: form.username.trim(),
    firstName: form.firstName.trim(),
    lastName: form.lastName.trim(),
    email: form.email.trim(),
    mobileNumber: form.mobileNumber.trim(),
    roleId: form.roleId || null,

});

const buildUpdatePayload = (form) => ({

    employeeCode: form.employeeCode.trim(),
    firstName: form.firstName.trim(),
    lastName: form.lastName.trim(),
    email: form.email.trim(),
    mobileNumber: form.mobileNumber.trim(),
    roleId: form.roleId || null,

});

const buildViewHtml = (data) => {

    const statusLabel = data.isActive ? "Active" : "Inactive";

    return `
        <div class="staff-view">
            <div class="staff-view-grid">
                <div class="staff-view-item">
                    <span>Employee Code</span>
                    <strong>${escapeHtml(data.employeeCode || "—")}</strong>
                </div>
                <div class="staff-view-item">
                    <span>Full Name</span>
                    <strong>${escapeHtml(
                        [data.firstName, data.lastName].filter(Boolean).join(" ") || "—"
                    )}</strong>
                </div>
                <div class="staff-view-item">
                    <span>Username</span>
                    <strong>${escapeHtml(data.username || "—")}</strong>
                </div>
                <div class="staff-view-item">
                    <span>Role</span>
                    <strong>${escapeHtml(data.role || "—")}</strong>
                </div>
                <div class="staff-view-item">
                    <span>Email</span>
                    <strong>${escapeHtml(data.email || "—")}</strong>
                </div>
                <div class="staff-view-item">
                    <span>Mobile</span>
                    <strong>${escapeHtml(data.mobileNumber || "—")}</strong>
                </div>
                <div class="staff-view-item">
                    <span>Status</span>
                    <strong>${statusLabel}</strong>
                </div>
                <div class="staff-view-item">
                    <span>Created</span>
                    <strong>${escapeHtml(formatDate(data.createdAt))}</strong>
                </div>
                <div class="staff-view-item">
                    <span>Updated</span>
                    <strong>${escapeHtml(formatDate(data.updatedAt))}</strong>
                </div>
            </div>
        </div>
    `;

};

const SearchableSelect = ({
    label,
    placeholder,
    options,
    value,
    onChange,
    onClear,
}) => {

    const [open, setOpen] = useState(false);

    const [filter, setFilter] = useState("");

    const selected = (options || []).find(
        (option) => option.id === value
    );

    const filtered = (options || []).filter(
        (option) =>
            option.name
                .toLowerCase()
                .includes(filter.toLowerCase())
    );

    return (

        <div className="staff-searchable">

            {label && (

                <label className="form-label">
                    {label}
                </label>

            )}

            <div className="staff-select-wrap">

                <button
                    type="button"
                    className="form-select staff-select-toggle"
                    onClick={() => setOpen((prev) => !prev)}
                >

                    <span>
                        {selected ? selected.name : placeholder}
                    </span>

                    <ChevronDown size={16} />

                </button>

                {selected && onClear && (

                    <button
                        type="button"
                        className="staff-select-clear"
                        title="Clear selection"
                        onClick={onClear}
                    >
                        <X size={14} />
                    </button>

                )}

            </div>

            {open && (

                <div className="staff-select-pop">

                    <input
                        autoFocus
                        type="text"
                        className="form-control staff-select-search"
                        placeholder="Type to search..."
                        value={filter}
                        onChange={(e) => setFilter(e.target.value)}
                    />

                    <ul className="staff-select-list">

                        {filtered.length === 0 ? (

                            <li className="staff-select-empty">
                                No matches found
                            </li>

                        ) : (

                            filtered.map((option) => (

                                <li key={option.id}>

                                    <button
                                        type="button"
                                        className={
                                            option.id === value
                                                ? "selected"
                                                : ""
                                        }
                                        onClick={() => {

                                            onChange(option.id);

                                            setOpen(false);

                                            setFilter("");

                                        }}
                                    >
                                        {option.name}
                                    </button>

                                </li>

                            ))

                        )}

                    </ul>

                </div>

            )}

        </div>

    );

};

export default function StaffManagementPage() {

    const [mode, setMode] = useState("list");

    const [staff, setStaff] = useState([]);

    const [totalPages, setTotalPages] = useState(1);

    const [totalCount, setTotalCount] = useState(0);

    const [page, setPage] = useState(1);

    const [pageSize] = useState(10);

    const [search, setSearch] = useState("");

    const [roleId, setRoleId] = useState("");

    const [status, setStatus] = useState("");

    const [query, setQuery] = useState({
        search: "",
        roleId: "",
        status: "",
    });

    const [loading, setLoading] = useState(true);

    const [saving, setSaving] = useState(false);

    const [opening, setOpening] = useState(false);

    const [editingStaffId, setEditingStaffId] = useState(null);

    const [form, setForm] = useState(toStaffForm(null));

    const [existingCodes, setExistingCodes] = useState([]);

    const [roles, setRoles] = useState([]);

    const loadRoles = useCallback(async () => {

        try {

            const response = await hospitalAdminService.getRoles({
                page: 1,
                pageSize: 100,
            });

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to load roles."
                );

                return;

            }

            const paged = normalized.data;

            const items = Array.isArray(paged?.items) ? paged.items : [];

            setRoles(
                items
                    .filter(
                        (role) =>
                            role.isActive &&
                            !NON_STAFF_ROLES.has(role.name)
                    )
                    .map((role) => ({
                        id: role.roleId,
                        name: role.name,
                    }))
                    .sort((a, b) => a.name.localeCompare(b.name))
            );

        }
        catch (error) {

            console.error(error);

            toast.error("Failed to load roles.");

        }

    }, []);

    useEffect(() => {

        const timer = setTimeout(loadRoles, 0);

        return () => clearTimeout(timer);

    }, [loadRoles]);

    useEffect(() => {

        const timer = setTimeout(() => {

            setPage(1);

            setQuery((prev) => {

                const next = {
                    search: search.trim(),
                    roleId,
                    status,
                };

                if (
                    prev.search === next.search &&
                    prev.roleId === next.roleId &&
                    prev.status === next.status
                ) {
                    return prev;
                }

                return next;

            });

        }, 400);

        return () => clearTimeout(timer);

    }, [search, roleId, status]);

    const loadStaff = useCallback(async () => {

        setLoading(true);

        try {

            const params = { page, pageSize };

            if (query.search) {
                params.search = query.search;
            }

            if (query.roleId) {
                params.roleId = query.roleId;
            }

            if (query.status) {
                params.status = query.status;
            }

            const response = await hospitalAdminService.getStaff(params);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to load staff."
                );

                setStaff([]);

                return;

            }

            const paged = normalized.data;

            setStaff(
                Array.isArray(paged?.items) ? paged.items : []
            );

            setTotalPages(
                typeof paged?.totalPages === "number"
                    ? paged.totalPages
                    : 1
            );

            setTotalCount(
                typeof paged?.totalCount === "number"
                    ? paged.totalCount
                    : 0
            );

        }
        catch (error) {

            console.error(error);

            toast.error(
                error?.response?.data?.message ||
                    "Something went wrong while loading staff."
            );

            setStaff([]);

        }
        finally {

            setLoading(false);

        }

    }, [page, query, pageSize]);

    useEffect(() => {

        const timer = setTimeout(loadStaff, 0);

        return () => clearTimeout(timer);

    }, [loadStaff]);

    const openCreate = async () => {

        setEditingStaffId(null);

        setForm(toStaffForm(null));

        setMode("create");

        try {

            // Fetch existing staff codes so the auto-generated preview
            // receives a unique suffix (RAHUL-PATIL, RAHUL-PATIL-2, ...).
            const response = await hospitalAdminService.getStaff({
                page: 1,
                pageSize: 100,
            });

            const normalized = normalizeResponse(response);

            if (!normalized.ok) return;

            const paged = normalized.data;

            if (Array.isArray(paged?.items)) {

                setExistingCodes(
                    paged.items.map((staffMember) => staffMember.employeeCode)
                );

            }

        }
        catch (error) {

            console.error(error);

        }

    };

    const openEdit = async (staffMember) => {

        setOpening(true);

        try {

            const response = await hospitalAdminService.getStaffById(
                staffMember.userId
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to load staff details."
                );

                return;

            }

            setEditingStaffId(staffMember.userId);

            setForm(toStaffForm(normalized.data));

            setMode("edit");

        }
        catch (error) {

            console.error(error);

            toast.error(
                "Something went wrong while loading the staff member."
            );

        }
        finally {

            setOpening(false);

        }

    };

    const cancelForm = () => {

        setEditingStaffId(null);

        setForm(toStaffForm(null));

        setMode("list");

    };

    const handleChange = (e) => {

        const { name, value } = e.target;

        if ((name === "firstName" || name === "lastName") && mode === "create") {

            const nextFirst = name === "firstName" ? value : form.firstName;

            const nextLast = name === "lastName" ? value : form.lastName;

            setForm((prev) => ({

                ...prev,

                [name]: value,

                employeeCode: uniqueCode(
                    generateCodeFromName(
                        `${nextFirst} ${nextLast}`.trim()
                    ),
                    existingCodes
                ),

                username: prev.username.trim()
                    ? prev.username
                    : suggestUsername(nextFirst, nextLast),

            }));

            return;

        }

        setForm((prev) => ({
            ...prev,
            [name]: value,
        }));

    };

    const handleSubmit = async (e) => {

        e.preventDefault();

        if (!form.firstName.trim()) {

            toast.error("First name is required.");

            return;

        }

        if (!form.lastName.trim()) {

            toast.error("Last name is required.");

            return;

        }

        if (!form.email.trim()) {

            toast.error("Email is required.");

            return;

        }

        if (!isValidEmail(form.email)) {

            toast.error("Please enter a valid email address.");

            return;

        }

        if (!form.mobileNumber.trim()) {

            toast.error("Mobile number is required.");

            return;

        }

        if (!isValidPhone(form.mobileNumber)) {

            toast.error("Please enter a valid mobile number.");

            return;

        }

        if (!form.roleId) {

            toast.error("Please select a role.");

            return;

        }

        if (mode === "create" && !form.username.trim()) {

            toast.error("Username is required.");

            return;

        }

        setSaving(true);

        try {

            const payload = mode === "edit"
                ? buildUpdatePayload(form)
                : buildCreatePayload(form);

            const response = mode === "edit"
                ? await hospitalAdminService.updateStaff(
                    editingStaffId,
                    payload
                )
                : await hospitalAdminService.createStaff(payload);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message ||
                        (mode === "edit"
                            ? "Failed to update staff."
                            : "Failed to create staff.")
                );

                return;

            }

            toast.success(
                normalized.message ||
                    (mode === "edit"
                        ? "Staff updated successfully."
                        : "Staff created successfully.")
            );

            cancelForm();

            loadStaff();

        }
        catch (error) {

            console.error(error);

            toast.error(
                error?.response?.data?.message ||
                    "Something went wrong."
            );

        }
        finally {

            setSaving(false);

        }

    };

    const handleStatusChange = async (staffMember) => {

        const isActive = staffMember.isActive;

        const result = await Swal.fire({

            title: isActive
                ? "Deactivate Staff Member?"
                : "Activate Staff Member?",

            text: isActive
                ? "The staff member will no longer be able to sign in."
                : "The staff member will be able to sign in again.",

            icon: "warning",

            showCancelButton: true,

            confirmButtonText: isActive
                ? "Yes, Deactivate"
                : "Yes, Activate",

            cancelButtonText: "Cancel",

            reverseButtons: true,

        });

        if (!result.isConfirmed) return;

        try {

            const response = isActive
                ? await hospitalAdminService.deactivateStaff(staffMember.userId)
                : await hospitalAdminService.activateStaff(staffMember.userId);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(normalized.message || "Something went wrong.");

                return;

            }

            toast.success(
                normalized.message ||
                    (isActive
                        ? "Staff deactivated successfully."
                        : "Staff activated successfully.")
            );

            loadStaff();

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };

    const handleDelete = async (staffMember) => {

        const result = await Swal.fire({

            title: "Delete Staff Member?",

            text: "The staff member and their login access will be removed. Historical records that reference this staff member are preserved.",

            icon: "warning",

            showCancelButton: true,

            confirmButtonText: "Yes, Delete",

            cancelButtonText: "Cancel",

            reverseButtons: true,

        });

        if (!result.isConfirmed) return;

        try {

            const response = await hospitalAdminService.deleteStaff(
                staffMember.userId
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(normalized.message || "Something went wrong.");

                return;

            }

            toast.success(
                normalized.message || "Staff deleted successfully."
            );

            if (staff.length === 1 && page > 1) {

                setPage(page - 1);

            }
            else {

                loadStaff();

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };

    const handleView = (staffMember) => {

        Swal.fire({

            title: staffMember.fullName || "Staff Details",

            html: buildViewHtml(staffMember),

            icon: "info",

            confirmButtonText: "Close",

            width: "min(640px, calc(100vw - 40px))",

        });

        (async () => {

            try {

                const response = await hospitalAdminService.getStaffById(
                    staffMember.userId
                );

                const normalized = normalizeResponse(response);

                if (!normalized.ok) {
                    return;
                }

                Swal.update({ html: buildViewHtml(normalized.data) });

            }
            catch (error) {

                console.error(error);

            }

        })();

    };

    if (mode === "create" || mode === "edit") {

        return (

            <div>

                <PageHeader
                    title={mode === "edit" ? "Edit Staff" : "Add Staff"}
                    subtitle={
                        mode === "edit"
                            ? "Update the staff member details and save your changes."
                            : "Add a new staff member for your hospital."
                    }
                />

                {opening ? (

                    <div className="staff-loading">
                        <Loader2 className="staff-spin" size={22} />
                        <span>Loading staff details...</span>
                    </div>

                ) : (

                    <div className="staff-form-card">

                        <form onSubmit={handleSubmit}>

                            <div className="staff-form-section">

                                <h5>Staff Information</h5>

                                <div className="row g-3">

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            First Name
                                        </label>

                                        <input
                                            autoFocus
                                            type="text"
                                            className="form-control"
                                            name="firstName"
                                            value={form.firstName}
                                            onChange={handleChange}
                                            placeholder="e.g. Rahul"
                                        />

                                    </div>

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Last Name
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="lastName"
                                            value={form.lastName}
                                            onChange={handleChange}
                                            placeholder="e.g. Patil"
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <SearchableSelect
                                            label="Role"
                                            placeholder="Select role"
                                            options={roles}
                                            value={form.roleId}
                                            onChange={(id) =>
                                                setForm((prev) => ({
                                                    ...prev,
                                                    roleId: id,
                                                }))
                                            }
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Mobile Number
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="mobileNumber"
                                            value={form.mobileNumber}
                                            onChange={handleChange}
                                            placeholder="e.g. 9876543210"
                                            maxLength={20}
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Email
                                        </label>

                                        <input
                                            type="email"
                                            className="form-control"
                                            name="email"
                                            value={form.email}
                                            onChange={handleChange}
                                            placeholder="Enter email"
                                        />

                                    </div>

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Employee Code
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control staff-code-preview"
                                            value={form.employeeCode}
                                            readOnly
                                            disabled
                                            placeholder="Auto-generated code"
                                        />

                                        <small className="staff-form-hint">
                                            Auto-generated from the name. A numeric suffix is added when the code is already in use.
                                        </small>

                                    </div>

                                </div>

                            </div>

                            <div className="staff-form-section">

                                <h5>Login Details</h5>

                                <div className="row g-3">

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Username
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="username"
                                            value={form.username}
                                            onChange={handleChange}
                                            placeholder="e.g. rahul.patil"
                                            disabled={mode === "edit"}
                                        />

                                        <small className="staff-form-hint">
                                            {
                                                mode === "edit"
                                                    ? "The username cannot be changed after creation."
                                                    : "Suggested from the name. The staff member uses this username to sign in."
                                            }
                                        </small>

                                    </div>

                                    <div className="col-md-6">

                                        <div className="staff-password-note">

                                            <div>
                                                <strong>Temporary Password</strong>
                                            </div>

                                            <p>
                                                A temporary password is generated automatically and shown once after the staff member is created. It is never shown again.
                                            </p>

                                        </div>

                                    </div>

                                </div>

                            </div>

                            <div className="staff-form-actions">

                                <button
                                    type="button"
                                    className="btn btn-light"
                                    onClick={cancelForm}
                                >

                                    <X size={16} />

                                    <span>Cancel</span>

                                </button>

                                <button
                                    type="submit"
                                    className="btn btn-primary"
                                    disabled={saving}
                                >

                                    {
                                        saving
                                            ? (
                                                <>
                                                    <Loader2
                                                        className="staff-spin"
                                                        size={16}
                                                    />
                                                    <span>Saving...</span>
                                                </>
                                            )
                                            : (
                                                <>
                                                    <Save size={16} />
                                                    <span>
                                                        {mode === "edit"
                                                            ? "Update Staff"
                                                            : "Save Staff"}
                                                    </span>
                                                </>
                                            )
                                    }

                                </button>

                            </div>

                        </form>

                    </div>

                )}

            </div>

        );

    }

    return (

        <>

            <PageHeader
                title="Staff"
                subtitle="Manage hospital staff and their access."
                action={

                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={openCreate}
                    >

                        <Plus size={18} />

                        <span>Add Staff</span>

                    </button>

                }
            />

            <div className="staff-table-card">

                <div className="staff-table-header">

                    <h3>Staff List</h3>

                    <span className="staff-count">
                        {totalCount} staff member{totalCount === 1 ? "" : "s"}
                    </span>

                </div>

                <div className="staff-filter">

                    <input
                        type="text"
                        className="form-control"
                        placeholder="Search staff..."
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />

                    <div className="staff-filter-role">

                        <SearchableSelect
                            label=""
                            placeholder="All Roles"
                            options={roles}
                            value={roleId}
                            onChange={setRoleId}
                            onClear={() => setRoleId("")}
                        />

                    </div>

                    <select
                        className="form-select"
                        value={status}
                        onChange={(e) => setStatus(e.target.value)}
                    >

                        <option value="">
                            All Status
                        </option>

                        <option value="true">
                            Active
                        </option>

                        <option value="false">
                            Inactive
                        </option>

                    </select>

                </div>

                {
                    loading ? (

                        <div className="staff-loading">
                            <Loader2 className="staff-spin" size={22} />
                            <span>Loading staff...</span>
                        </div>

                    ) : staff.length === 0 ? (

                        <div className="staff-empty">

                            <Users size={40} />

                            <h4>No staff yet</h4>

                            <p>
                                Add your first staff member to get started.
                            </p>

                            <button
                                type="button"
                                className="btn btn-primary"
                                onClick={openCreate}
                            >

                                <Plus size={16} />

                                <span>Add Staff</span>

                            </button>

                        </div>

                    ) : (

                        <div className="table-responsive">

                            <table className="table staff-table">

                                <thead>

                                    <tr>

                                        <th>Employee Code</th>
                                        <th>Name</th>
                                        <th>Username</th>
                                        <th>Role</th>
                                        <th>Email</th>
                                        <th>Mobile</th>
                                        <th>Status</th>
                                        <th>Created</th>
                                        <th width="220">Actions</th>

                                    </tr>

                                </thead>

                                <tbody>

                                    {
                                        staff.map((staffMember) => (

                                            <tr key={staffMember.userId}>

                                                <td className="staff-code">
                                                    {staffMember.employeeCode}
                                                </td>

                                                <td>

                                                    <strong>
                                                        {staffMember.fullName}
                                                    </strong>

                                                </td>

                                                <td className="staff-username">
                                                    {staffMember.username || "—"}
                                                </td>

                                                <td>
                                                    {staffMember.role || "—"}
                                                </td>

                                                <td>
                                                    {staffMember.email || "—"}
                                                </td>

                                                <td>
                                                    {staffMember.mobileNumber || "—"}
                                                </td>

                                                <td>

                                                    <span
                                                        className={
                                                            staffMember.isActive
                                                                ? "badge-active"
                                                                : "badge-inactive"
                                                        }
                                                    >

                                                        {staffMember.isActive
                                                            ? "Active"
                                                            : "Inactive"}

                                                    </span>

                                                </td>

                                                <td>
                                                    {formatDate(staffMember.createdAt)}
                                                </td>

                                                <td>

                                                    <div className="action-buttons">

                                                        <button
                                                            className="action-btn view"
                                                            onClick={() => handleView(staffMember)}
                                                            title="View"
                                                        >
                                                            <Eye size={16} />
                                                        </button>

                                                        <button
                                                            className="action-btn edit"
                                                            onClick={() => openEdit(staffMember)}
                                                            title="Edit"
                                                        >
                                                            <Pencil size={16} />
                                                        </button>

                                                        <button
                                                            className={
                                                                staffMember.isActive
                                                                    ? "action-btn deactivate"
                                                                    : "action-btn activate"
                                                            }
                                                            onClick={() => handleStatusChange(staffMember)}
                                                            title={
                                                                staffMember.isActive
                                                                    ? "Deactivate"
                                                                    : "Activate"
                                                            }
                                                        >
                                                            <Power size={16} />
                                                        </button>

                                                        <button
                                                            className="action-btn delete"
                                                            onClick={() => handleDelete(staffMember)}
                                                            title="Delete"
                                                        >
                                                            <Trash2 size={16} />
                                                        </button>

                                                    </div>

                                                </td>

                                            </tr>

                                        ))
                                    }

                                </tbody>

                            </table>

                            <Pagination
                                currentPage={page}
                                totalPages={totalPages}
                                onPageChange={setPage}
                            />

                        </div>

                    )
                }

            </div>

        </>

    );

}