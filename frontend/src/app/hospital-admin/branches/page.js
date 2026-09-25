"use client";

import { useCallback, useEffect, useState } from "react";

import {
    Plus,
    Pencil,
    Eye,
    Power,
    Trash2,
    Building2,
    X,
    Save,
    Loader2,
} from "lucide-react";

import { toast } from "react-toastify";
import Swal from "sweetalert2";

import hospitalAdminService from "@/services/hospital-admin/hospitalAdminService";
import PageHeader from "@/components/super-admin/PageHeader";
import Pagination from "@/components/common/Pagination";
import states from "@/data/states";
import {
    generateCodeFromName,
    uniqueCode,
} from "@/utils/codeGenerator";

import "./Branches.css";

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

const toBranchForm = (branch) => ({

    name: branch?.name || "",
    code: branch?.code || "",
    email: branch?.email || "",
    phoneNumber: branch?.phoneNumber || "",
    address: branch?.address || "",
    city: branch?.city || "",
    state: branch?.state || "",
    country: branch?.country || "India",
    postalCode: branch?.postalCode || "",
    isHeadOffice: !!branch?.isHeadOffice,

});

const buildPayload = (form) => ({

    name: form.name.trim(),
    code: form.code.trim(),
    email: form.email.trim() || null,
    phoneNumber: form.phoneNumber.trim() || null,
    address: form.address.trim(),
    city: form.city.trim(),
    state: form.state,
    country: form.country.trim() || "India",
    postalCode: form.postalCode.trim() || null,
    isHeadOffice: !!form.isHeadOffice,

});

const buildViewHtml = (data) => {

    const statusLabel = data.isActive ? "Active" : "Inactive";

    const headOfficeLabel = data.isHeadOffice ? "Yes" : "No";

    return `
        <div class="branch-view">
            <div class="branch-view-grid">
                <div class="branch-view-item">
                    <span>Branch Code</span>
                    <strong>${escapeHtml(data.code || "—")}</strong>
                </div>
                <div class="branch-view-item">
                    <span>Branch Name</span>
                    <strong>${escapeHtml(data.name || "—")}</strong>
                </div>
                <div class="branch-view-item">
                    <span>Email</span>
                    <strong>${escapeHtml(data.email || "—")}</strong>
                </div>
                <div class="branch-view-item">
                    <span>Phone</span>
                    <strong>${escapeHtml(data.phoneNumber || "—")}</strong>
                </div>
                <div class="branch-view-item branch-view-item-wide">
                    <span>Address</span>
                    <strong>${escapeHtml(data.address || "—")}</strong>
                </div>
                <div class="branch-view-item">
                    <span>City</span>
                    <strong>${escapeHtml(data.city || "—")}</strong>
                </div>
                <div class="branch-view-item">
                    <span>State</span>
                    <strong>${escapeHtml(data.state || "—")}</strong>
                </div>
                <div class="branch-view-item">
                    <span>Country</span>
                    <strong>${escapeHtml(data.country || "—")}</strong>
                </div>
                <div class="branch-view-item">
                    <span>Postal Code</span>
                    <strong>${escapeHtml(data.postalCode || "—")}</strong>
                </div>
                <div class="branch-view-item">
                    <span>Head Office</span>
                    <strong>${headOfficeLabel}</strong>
                </div>
                <div class="branch-view-item">
                    <span>Status</span>
                    <strong>${statusLabel}</strong>
                </div>
                <div class="branch-view-item">
                    <span>Created At</span>
                    <strong>${escapeHtml(formatDate(data.createdAt))}</strong>
                </div>
            </div>
        </div>
    `;

};

export default function BranchManagementPage() {

    const [mode, setMode] = useState("list");

    const [branches, setBranches] = useState([]);

    const [totalPages, setTotalPages] = useState(1);

    const [totalCount, setTotalCount] = useState(0);

    const [page, setPage] = useState(1);

    const [pageSize] = useState(10);

    const [search, setSearch] = useState("");

    const [status, setStatus] = useState("");

    const [query, setQuery] = useState({ search: "", status: "" });

    const [loading, setLoading] = useState(true);

    const [saving, setSaving] = useState(false);

    const [opening, setOpening] = useState(false);

    const [editingBranchId, setEditingBranchId] = useState(null);

    const [form, setForm] = useState(toBranchForm(null));

    const [existingBranchCodes, setExistingBranchCodes] = useState([]);

    useEffect(() => {

        const timer = setTimeout(() => {

            setPage(1);

            setQuery((prev) => {

                const next = {
                    search: search.trim(),
                    status,
                };

                if (
                    prev.search === next.search &&
                    prev.status === next.status
                ) {
                    return prev;
                }

                return next;

            });

        }, 400);

        return () => clearTimeout(timer);

    }, [search, status]);

    const loadBranches = useCallback(async () => {

        setLoading(true);

        try {

            const params = { page, pageSize };

            if (query.search) {
                params.search = query.search;
            }

            if (query.status) {
                params.status = query.status;
            }

            const response = await hospitalAdminService.getBranches(params);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to load branches."
                );

                setBranches([]);

                return;

            }

            const paged = normalized.data;

            setBranches(
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
                    "Something went wrong while loading branches."
            );

            setBranches([]);

        }
        finally {

            setLoading(false);

        }

    }, [page, query, pageSize]);

    useEffect(() => {

        const timer = setTimeout(loadBranches, 0);

        return () => clearTimeout(timer);

    }, [loadBranches]);

    const openCreate = async () => {

        setEditingBranchId(null);

        setForm(toBranchForm(null));

        setExistingBranchCodes([]);

        setMode("create");

        try {

            // Fetch existing codes so the auto-generated preview
            // receives a unique suffix (KOLHAPUR, KOLHAPUR-2, ...).
            const response = await hospitalAdminService.getBranches({
                page: 1,
                pageSize: 100,
            });

            const normalized = normalizeResponse(response);

            if (!normalized.ok) return;

            const paged = normalized.data;

            if (Array.isArray(paged?.items)) {

                setExistingBranchCodes(
                    paged.items.map((branch) => branch.code)
                );

            }

        }
        catch (error) {

            console.error(error);

        }

    };

    const openEdit = async (branch) => {

        setOpening(true);

        try {

            const response = await hospitalAdminService.getBranchById(
                branch.branchId
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to load branch details."
                );

                return;

            }

            setEditingBranchId(branch.branchId);

            setForm(toBranchForm(normalized.data));

            setMode("edit");

        }
        catch (error) {

            console.error(error);

            toast.error(
                "Something went wrong while loading the branch."
            );

        }
        finally {

            setOpening(false);

        }

    };

    const cancelForm = () => {

        setEditingBranchId(null);

        setForm(toBranchForm(null));

        setMode("list");

    };

    const handleChange = (e) => {

        const { name, type, checked, value } = e.target;

        if (name === "name" && mode === "create") {

            const baseCode = generateCodeFromName(value);

            setForm((prev) => ({
                ...prev,
                name: value,
                code: uniqueCode(baseCode, existingBranchCodes),
            }));

            return;

        }

        setForm((prev) => ({
            ...prev,
            [name]: type === "checkbox" ? checked : value,
        }));

    };

    const handleSubmit = async (e) => {

        e.preventDefault();

        if (!form.name.trim()) {

            toast.error("Branch name is required.");

            return;

        }

        if (!form.code.trim()) {

            toast.error("Code is required.");

            return;

        }

        if (!form.address.trim()) {

            toast.error("Address is required.");

            return;

        }

        if (!form.city.trim()) {

            toast.error("City is required.");

            return;

        }

        if (!form.state) {

            toast.error("State is required.");

            return;

        }

        if (!form.country.trim()) {

            toast.error("Country is required.");

            return;

        }

        setSaving(true);

        try {

            const payload = buildPayload(form);

            const response = mode === "edit"
                ? await hospitalAdminService.updateBranch(
                    editingBranchId,
                    payload
                )
                : await hospitalAdminService.createBranch(payload);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message ||
                        (mode === "edit"
                            ? "Failed to update branch."
                            : "Failed to create branch.")
                );

                return;

            }

            toast.success(
                normalized.message ||
                    (mode === "edit"
                        ? "Branch updated successfully."
                        : "Branch created successfully.")
            );

            cancelForm();

            loadBranches();

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

    const handleStatusChange = async (branch) => {

        const isActive = branch.isActive;

        const result = await Swal.fire({

            title: isActive
                ? "Deactivate Branch?"
                : "Activate Branch?",

            text: isActive
                ? "The branch will become inactive."
                : "The branch will become active.",

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
                ? await hospitalAdminService.deactivateBranch(branch.branchId)
                : await hospitalAdminService.activateBranch(branch.branchId);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(normalized.message || "Something went wrong.");

                return;

            }

            toast.success(
                normalized.message ||
                    (isActive
                        ? "Branch deactivated successfully."
                        : "Branch activated successfully.")
            );

            loadBranches();

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };

    const handleDelete = async (branch) => {

        const result = await Swal.fire({

            title: "Delete Branch?",

            text: "This branch will be deleted. The backend performs a soft delete, so historical records that reference this branch are preserved.",

            icon: "warning",

            showCancelButton: true,

            confirmButtonText: "Yes, Delete",

            cancelButtonText: "Cancel",

            reverseButtons: true,

        });

        if (!result.isConfirmed) return;

        try {

            const response = await hospitalAdminService.deleteBranch(
                branch.branchId
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(normalized.message || "Something went wrong.");

                return;

            }

            toast.success(
                normalized.message || "Branch deleted successfully."
            );

            if (branches.length === 1 && page > 1) {

                setPage(page - 1);

            }
            else {

                loadBranches();

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };

    const handleView = (branch) => {

        Swal.fire({

            title: branch.name || "Branch Details",

            html: buildViewHtml(branch),

            icon: "info",

            confirmButtonText: "Close",

            width: 640,

        });

        (async () => {

            try {

                const response = await hospitalAdminService.getBranchById(
                    branch.branchId
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
                    title={mode === "edit" ? "Edit Branch" : "Add Branch"}
                    subtitle={
                        mode === "edit"
                            ? "Update the branch details and save your changes."
                            : "Add a new branch for your hospital."
                    }
                />

                {opening ? (

                    <div className="branches-loading">
                        <Loader2 className="branches-spin" size={22} />
                        <span>Loading branch details...</span>
                    </div>

                ) : (

                    <div className="branches-form-card">

                        <form onSubmit={handleSubmit}>

                            <div className="branches-form-section">

                                <h5>Branch Information</h5>

                                <div className="row g-3">

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Branch Name
                                        </label>

                                        <input
                                            autoFocus
                                            type="text"
                                            className="form-control"
                                            name="name"
                                            value={form.name}
                                            onChange={handleChange}
                                            placeholder="Enter branch name"
                                        />

                                    </div>

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Branch Code
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control branch-code-preview"
                                            value={form.code}
                                            readOnly
                                            disabled
                                            placeholder="Auto-generated code"
                                        />

                                        <small className="branches-form-hint">
                                            {
                                                mode === "edit"
                                                    ? "This code is preserved from the existing record."
                                                    : "Auto-generated from the branch name. A numeric suffix is added when the code is already in use."
                                            }
                                        </small>

                                    </div>

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Email
                                        </label>

                                        <input
                                            type="email"
                                            className="form-control"
                                            name="email"
                                            value={form.email}
                                            onChange={handleChange}
                                            placeholder="Enter branch email"
                                        />

                                    </div>

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Phone Number
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="phoneNumber"
                                            value={form.phoneNumber}
                                            onChange={handleChange}
                                            placeholder="Enter phone number"
                                            maxLength={20}
                                        />

                                    </div>

                                    <div className="col-12">

                                        <div className="form-check form-switch">

                                            <input
                                                className="form-check-input"
                                                type="checkbox"
                                                name="isHeadOffice"
                                                id="branchIsHeadOffice"
                                                checked={form.isHeadOffice}
                                                onChange={handleChange}
                                            />

                                            <label
                                                className="form-check-label"
                                                htmlFor="branchIsHeadOffice"
                                            >
                                                Head Office
                                            </label>

                                        </div>

                                        <small className="branches-form-hint">
                                            If enabled, the previous head
                                            office branch is automatically
                                            demoted.
                                        </small>

                                    </div>

                                </div>

                            </div>

                            <div className="branches-form-section">

                                <h5>Address Information</h5>

                                <div className="row g-3">

                                    <div className="col-12">

                                        <label className="form-label">
                                            Address
                                        </label>

                                        <textarea
                                            rows="3"
                                            className="form-control"
                                            name="address"
                                            value={form.address}
                                            onChange={handleChange}
                                            placeholder="Enter address"
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            City
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="city"
                                            value={form.city}
                                            onChange={handleChange}
                                            placeholder="Enter city"
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            State
                                        </label>

                                        <select
                                            className="form-select"
                                            name="state"
                                            value={form.state}
                                            onChange={handleChange}
                                        >

                                            <option value="">
                                                Select State
                                            </option>

                                            {states.map((state) => (

                                                <option
                                                    key={state}
                                                    value={state}
                                                >
                                                    {state}
                                                </option>

                                            ))}

                                        </select>

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Country
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="country"
                                            value={form.country}
                                            onChange={handleChange}
                                            placeholder="Enter country"
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Postal Code
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="postalCode"
                                            value={form.postalCode}
                                            onChange={handleChange}
                                            placeholder="Enter postal code"
                                            maxLength={20}
                                        />

                                    </div>

                                </div>

                            </div>

                            <div className="branches-form-actions">

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
                                                        className="branches-spin"
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
                                                            ? "Update Branch"
                                                            : "Save Branch"}
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
                title="Branches"
                subtitle="Manage your hospital branches and locations."
                action={

                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={openCreate}
                    >

                        <Plus size={18} />

                        <span>Add Branch</span>

                    </button>

                }
            />

            <div className="branches-table-card">

                <div className="branches-table-header">

                    <h3>Branch List</h3>

                    <span className="branches-count">
                        {totalCount} branch{totalCount === 1 ? "" : "es"}
                    </span>

                </div>

                <div className="branches-filter">

                    <input
                        type="text"
                        className="form-control"
                        placeholder="Search branches..."
                        value={search}
                        onChange={(e) => setSearch(e.target.value)}
                    />

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

                        <div className="branches-loading">
                            <Loader2 className="branches-spin" size={22} />
                            <span>Loading branches...</span>
                        </div>

                    ) : branches.length === 0 ? (

                        <div className="branches-empty">

                            <Building2 size={40} />

                            <h4>No branches yet</h4>

                            <p>
                                Add your first branch to start organizing
                                your hospital locations.
                            </p>

                            <button
                                type="button"
                                className="btn btn-primary"
                                onClick={openCreate}
                            >

                                <Plus size={16} />

                                <span>Add Branch</span>

                            </button>

                        </div>

                    ) : (

                        <div className="table-responsive">

                            <table className="table branches-table">

                                <thead>

                                    <tr>

                                        <th>Code</th>
                                        <th>Branch Name</th>
                                        <th>Phone</th>
                                        <th>City</th>
                                        <th>State</th>
                                        <th>Status</th>
                                        <th>Created</th>
                                        <th width="160">Actions</th>

                                    </tr>

                                </thead>

                                <tbody>

                                    {
                                        branches.map((branch) => (

                                            <tr key={branch.branchId}>

                                                <td className="branches-code">
                                                    {branch.code}
                                                </td>

                                                <td>

                                                    <strong>
                                                        {branch.name}
                                                    </strong>

                                                    {
                                                        branch.isHeadOffice && (

                                                            <span className="badge-head-office">
                                                                Head Office
                                                            </span>

                                                        )
                                                    }

                                                </td>

                                                <td>
                                                    {branch.phoneNumber || "—"}
                                                </td>

                                                <td>
                                                    {branch.city || "—"}
                                                </td>

                                                <td>
                                                    {branch.state || "—"}
                                                </td>

                                                <td>

                                                    <span
                                                        className={
                                                            branch.isActive
                                                                ? "badge-active"
                                                                : "badge-inactive"
                                                        }
                                                    >

                                                        {branch.isActive
                                                            ? "Active"
                                                            : "Inactive"}

                                                    </span>

                                                </td>

                                                <td>
                                                    {formatDate(branch.createdAt)}
                                                </td>

                                                <td>

                                                    <div className="action-buttons">

                                                        <button
                                                            className="action-btn view"
                                                            onClick={() => handleView(branch)}
                                                            title="View"
                                                        >
                                                            <Eye size={16} />
                                                        </button>

                                                        <button
                                                            className="action-btn edit"
                                                            onClick={() => openEdit(branch)}
                                                            title="Edit"
                                                        >
                                                            <Pencil size={16} />
                                                        </button>

                                                        <button
                                                            className={
                                                                branch.isActive
                                                                    ? "action-btn deactivate"
                                                                    : "action-btn activate"
                                                            }
                                                            onClick={() => handleStatusChange(branch)}
                                                            title={
                                                                branch.isActive
                                                                    ? "Deactivate"
                                                                    : "Activate"
                                                            }
                                                        >
                                                            <Power size={16} />
                                                        </button>

                                                        <button
                                                            className="action-btn delete"
                                                            onClick={() => handleDelete(branch)}
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