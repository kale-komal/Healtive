"use client";

import { useCallback, useEffect, useState } from "react";

import {
    Plus,
    Pencil,
    Eye,
    Power,
    Trash2,
    Layers,
    X,
    Save,
    Loader2,
} from "lucide-react";

import { toast } from "react-toastify";
import Swal from "sweetalert2";

import hospitalAdminService from "@/services/hospital-admin/hospitalAdminService";
import PageHeader from "@/components/super-admin/PageHeader";
import Pagination from "@/components/common/Pagination";
import { COMMON_DEPARTMENTS } from "@/data/commonDepartments";
import {
    generateCodeFromName,
    uniqueCode,
} from "@/utils/codeGenerator";

import "./Departments.css";

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

const successText = (normalized, fallback) => {

    const data = normalized?.data;

    return typeof data === "string" && data.trim()
        ? data.trim()
        : fallback;

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

const toDepartmentForm = (department) => ({

    name: department?.name || "",
    code: department?.code || "",
    description: department?.description || "",

});

const buildPayload = (form) => ({

    name: form.name.trim(),
    code: form.code.trim(),
    description: form.description.trim() || null,

});

const buildViewHtml = (data) => {

    const statusLabel = data.isActive ? "Active" : "Inactive";

    return `
        <div class="department-view">
            <div class="department-view-grid">
                <div class="department-view-item">
                    <span>Department Code</span>
                    <strong>${escapeHtml(data.code || "—")}</strong>
                </div>
                <div class="department-view-item">
                    <span>Department Name</span>
                    <strong>${escapeHtml(data.name || "—")}</strong>
                </div>
                <div class="department-view-item department-view-item-wide">
                    <span>Description</span>
                    <strong>${escapeHtml(data.description || "—")}</strong>
                </div>
                <div class="department-view-item">
                    <span>Status</span>
                    <strong>${statusLabel}</strong>
                </div>
                <div class="department-view-item">
                    <span>Created At</span>
                    <strong>${escapeHtml(formatDate(data.createdAt))}</strong>
                </div>
            </div>
        </div>
    `;

};

export default function DepartmentManagementPage() {

    const [mode, setMode] = useState("list");

    const [departments, setDepartments] = useState([]);

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

    const [editingDepartmentId, setEditingDepartmentId] = useState(null);

    const [form, setForm] = useState(toDepartmentForm(null));

    const [isCustom, setIsCustom] = useState(false);

    const [catalogSearch, setCatalogSearch] = useState("");

    const [catalogSelected, setCatalogSelected] = useState("");

    const [existingDepartments, setExistingDepartments] = useState([]);

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

    const loadDepartments = useCallback(async () => {

        setLoading(true);

        try {

            const params = { page, pageSize };

            if (query.search) {
                params.search = query.search;
            }

            if (query.status) {
                params.status = query.status;
            }

            const response = await hospitalAdminService.getDepartments(params);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to load departments."
                );

                setDepartments([]);

                return;

            }

            const paged = normalized.data;

            setDepartments(
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
                    "Something went wrong while loading departments."
            );

            setDepartments([]);

        }
        finally {

            setLoading(false);

        }

    }, [page, query, pageSize]);

    useEffect(() => {

        const timer = setTimeout(loadDepartments, 0);

        return () => clearTimeout(timer);

    }, [loadDepartments]);

    const openCreate = async () => {

        setEditingDepartmentId(null);

        setForm(toDepartmentForm(null));

        setIsCustom(false);

        setCatalogSearch("");

        setCatalogSelected("");

        setExistingDepartments([]);

        setMode("create");

        try {

            // Fetch existing departments so already-added common
            // departments are hidden from the catalog.
            const response = await hospitalAdminService.getDepartments({
                page: 1,
                pageSize: 100,
            });

            const normalized = normalizeResponse(response);

            if (!normalized.ok) return;

            const paged = normalized.data;

            if (Array.isArray(paged?.items)) {

                setExistingDepartments(paged.items);

            }

        }
        catch (error) {

            console.error(error);

        }

    };

    const openEdit = async (department) => {

        setOpening(true);

        try {

            const response = await hospitalAdminService.getDepartmentById(
                department.departmentId
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to load department details."
                );

                return;

            }

            setEditingDepartmentId(department.departmentId);

            setForm(toDepartmentForm(normalized.data));

            setMode("edit");

        }
        catch (error) {

            console.error(error);

            toast.error(
                "Something went wrong while loading the department."
            );

        }
        finally {

            setOpening(false);

        }

    };

    const cancelForm = () => {

        setEditingDepartmentId(null);

        setForm(toDepartmentForm(null));

        setIsCustom(false);

        setCatalogSearch("");

        setCatalogSelected("");

        setMode("list");

    };

    const handleChange = (e) => {

        const { name, value } = e.target;

        if (name === "name" && mode === "create") {

            const baseCode = generateCodeFromName(value);

            setForm((prev) => {

                const next = { ...prev, name: value };

                if (isCustom || !prev.code) {

                    next.code = uniqueCode(
                        baseCode,
                        existingDepartments.map(
                            (department) => department.code
                        )
                    );

                }

                return next;

            });

            return;

        }

        setForm((prev) => ({
            ...prev,
            [name]: value,
        }));

    };

    const handleCatalogSelect = (e) => {

        const code = e.target.value;

        setCatalogSelected(code);

        if (!code) {

            setForm((prev) => ({
                ...prev,
                name: "",
                code: "",
                description: "",
            }));

            return;

        }

        const item = COMMON_DEPARTMENTS.find(
            (department) => department.code === code
        );

        if (!item) return;

        setForm({
            name: item.name,
            code: item.code,
            description: item.description,
        });

    };

    const chooseCatalog = () => {

        setIsCustom(false);

        setCatalogSelected("");

        setForm(toDepartmentForm(null));

    };

    const chooseCustom = () => {

        setIsCustom(true);

        setCatalogSelected("");

        setForm(toDepartmentForm(null));

    };

    const handleSubmit = async (e) => {

        e.preventDefault();

        if (!form.name.trim()) {

            toast.error("Department name is required.");

            return;

        }

        if (!form.code.trim()) {

            toast.error("Department code is required.");

            return;

        }

        setSaving(true);

        try {

            const payload = buildPayload(form);

            const response = mode === "edit"
                ? await hospitalAdminService.updateDepartment(
                    editingDepartmentId,
                    payload
                )
                : await hospitalAdminService.createDepartment(payload);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message ||
                        (mode === "edit"
                            ? "Failed to update department."
                            : "Failed to create department.")
                );

                return;

            }

            toast.success(
                successText(
                    normalized,
                    mode === "edit"
                        ? "Department updated successfully."
                        : "Department created successfully."
                )
            );

            cancelForm();

            loadDepartments();

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

    const handleStatusChange = async (department) => {

        const isActive = department.isActive;

        const result = await Swal.fire({

            title: isActive
                ? "Deactivate Department?"
                : "Activate Department?",

            text: isActive
                ? "The department will become inactive."
                : "The department will become active.",

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
                ? await hospitalAdminService.deactivateDepartment(
                    department.departmentId
                )
                : await hospitalAdminService.activateDepartment(
                    department.departmentId
                );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(normalized.message || "Something went wrong.");

                return;

            }

            toast.success(
                successText(
                    normalized,
                    isActive
                        ? "Department deactivated successfully."
                        : "Department activated successfully."
                )
            );

            loadDepartments();

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };

    const handleDelete = async (department) => {

        const result = await Swal.fire({

            title: "Delete Department?",

            text: "This department will be deleted. The backend performs a soft delete, so historical records that reference this department are preserved.",

            icon: "warning",

            showCancelButton: true,

            confirmButtonText: "Yes, Delete",

            cancelButtonText: "Cancel",

            reverseButtons: true,

        });

        if (!result.isConfirmed) return;

        try {

            const response = await hospitalAdminService.deleteDepartment(
                department.departmentId
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(normalized.message || "Something went wrong.");

                return;

            }

            toast.success(
                successText(normalized, "Department deleted successfully.")
            );

            if (departments.length === 1 && page > 1) {

                setPage(page - 1);

            }
            else {

                loadDepartments();

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };

    const handleView = (department) => {

        Swal.fire({

            title: department.name || "Department Details",

            html: buildViewHtml(department),

            icon: "info",

            confirmButtonText: "Close",

            width: 640,

        });

    };

    const existingKeys = new Set(
        existingDepartments.flatMap((department) => [
            String(department.name ?? "").toLowerCase(),
            String(department.code ?? "").toLowerCase(),
        ])
    );

    const availableCatalog = COMMON_DEPARTMENTS.filter(
        (item) =>
            !existingKeys.has(item.name.toLowerCase()) &&
            !existingKeys.has(item.code.toLowerCase())
    );

    const filteredCatalog = availableCatalog.filter((item) =>
        item.name.toLowerCase().includes(
            catalogSearch.trim().toLowerCase()
        )
    );

    if (mode === "create" || mode === "edit") {

        return (

            <div>

                <PageHeader
                    title={mode === "edit" ? "Edit Department" : "Add Department"}
                    subtitle={
                        mode === "edit"
                            ? "Update the department details and save your changes."
                            : "Add a new department for your hospital."
                    }
                />

                {opening ? (

                    <div className="departments-loading">
                        <Loader2 className="departments-spin" size={22} />
                        <span>Loading department details...</span>
                    </div>

                ) : (

                    <div className="departments-form-card">

                        <form onSubmit={handleSubmit}>

                            <div className="departments-form-section">

                                {
                                    mode === "create" && (
                                        <div className="departments-source">

                                            <h5>Department Source</h5>

                                            <div className="departments-source-toggle">

                                                <button
                                                    type="button"
                                                    className={
                                                        isCustom
                                                            ? "btn btn-light"
                                                            : "btn btn-primary"
                                                    }
                                                    onClick={chooseCatalog}
                                                >

                                                    <Layers size={16} />

                                                    <span>
                                                        Common Department
                                                    </span>

                                                </button>

                                                <button
                                                    type="button"
                                                    className={
                                                        isCustom
                                                            ? "btn btn-primary"
                                                            : "btn btn-light"
                                                    }
                                                    onClick={chooseCustom}
                                                >

                                                    <Plus size={16} />

                                                    <span>
                                                        Create Custom
                                                    </span>

                                                </button>

                                            </div>

                                            {
                                                !isCustom && (
                                                    <div className="departments-catalog">

                                                        <input
                                                            autoFocus
                                                            type="text"
                                                            className="form-control"
                                                            placeholder="Search common departments..."
                                                            value={catalogSearch}
                                                            onChange={(e) => {
                                                                setCatalogSearch(e.target.value);
                                                            }}
                                                        />

                                                        <select
                                                            className="form-select"
                                                            value={catalogSelected}
                                                            onChange={handleCatalogSelect}
                                                        >

                                                            <option value="">
                                                                Select a common department...
                                                            </option>

                                                            {
                                                                filteredCatalog.map((item) => (

                                                                    <option
                                                                        key={item.code}
                                                                        value={item.code}
                                                                    >
                                                                        {item.name} ({item.code})
                                                                    </option>

                                                                ))
                                                            }

                                                        </select>

                                                        {
                                                            availableCatalog.length === 0 && (
                                                                <small className="departments-form-hint">
                                                                    All common departments are already
                                                                    added for this hospital. Use Create
                                                                    Custom to add a new department.
                                                                </small>
                                                            )
                                                        }

                                                        {
                                                            availableCatalog.length > 0 &&
                                                            filteredCatalog.length === 0 && (
                                                                <small className="departments-form-hint">
                                                                    No common departments match your
                                                                    search.
                                                                </small>
                                                            )
                                                        }

                                                    </div>
                                                )
                                            }

                                        </div>
                                    )
                                }

                            </div>

                            <div className="departments-form-section">

                                <h5>Department Information</h5>

                                <div className="row g-3">

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Department Name
                                        </label>

                                        <input
                                            autoFocus={mode === "edit" || isCustom}
                                            type="text"
                                            className="form-control"
                                            name="name"
                                            value={form.name}
                                            onChange={handleChange}
                                            placeholder={
                                                mode === "create" &&
                                                !isCustom
                                                    ? "Select a common department above or type a name"
                                                    : "Enter department name"
                                            }
                                        />

                                    </div>

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Department Code
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control department-code-preview"
                                            value={form.code}
                                            readOnly
                                            disabled
                                            placeholder="Auto-generated code"
                                        />

                                        <small className="departments-form-hint">
                                            {
                                                mode === "edit"
                                                    ? "This code is preserved from the existing record."
                                                    : isCustom
                                                        ? "Auto-generated from the department name. A numeric suffix is added when the code is already in use."
                                                        : "Auto-filled from the selected common department."
                                            }
                                        </small>

                                    </div>

                                    <div className="col-12">

                                        <label className="form-label">
                                            Description
                                        </label>

                                        <textarea
                                            rows="4"
                                            className="form-control"
                                            name="description"
                                            value={form.description}
                                            onChange={handleChange}
                                            placeholder="Enter a description (optional)"
                                        />

                                    </div>

                                </div>

                            </div>

                            <div className="departments-form-actions">

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
                                                        className="departments-spin"
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
                                                            ? "Update Department"
                                                            : "Save Department"}
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
                title="Departments"
                subtitle="Manage your hospital departments and services."
                action={

                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={openCreate}
                    >

                        <Plus size={18} />

                        <span>Add Department</span>

                    </button>

                }
            />

            <div className="departments-table-card">

                <div className="departments-table-header">

                    <h3>Department List</h3>

                    <span className="departments-count">
                        {totalCount} department
                        {totalCount === 1 ? "" : "s"}
                    </span>

                </div>

                <div className="departments-filter">

                    <input
                        type="text"
                        className="form-control"
                        placeholder="Search departments..."
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

                        <div className="departments-loading">
                            <Loader2 className="departments-spin" size={22} />
                            <span>Loading departments...</span>
                        </div>

                    ) : departments.length === 0 ? (

                        <div className="departments-empty">

                            <Layers size={40} />

                            <h4>No departments yet</h4>

                            <p>
                                Add your first department to start organizing
                                your hospital services.
                            </p>

                            <button
                                type="button"
                                className="btn btn-primary"
                                onClick={openCreate}
                            >

                                <Plus size={16} />

                                <span>Add Department</span>

                            </button>

                        </div>

                    ) : (

                        <div className="table-responsive">

                            <table className="table departments-table">

                                <thead>

                                    <tr>

                                        <th>Code</th>
                                        <th>Department Name</th>
                                        <th>Description</th>
                                        <th>Status</th>
                                        <th>Created</th>
                                        <th width="160">Actions</th>

                                    </tr>

                                </thead>

                                <tbody>

                                    {
                                        departments.map((department) => (

                                            <tr key={department.departmentId}>

                                                <td className="departments-code">
                                                    {department.code}
                                                </td>

                                                <td>
                                                    <strong>
                                                        {department.name}
                                                    </strong>
                                                </td>

                                                <td className="departments-desc">
                                                    {department.description || "—"}
                                                </td>

                                                <td>

                                                    <span
                                                        className={
                                                            department.isActive
                                                                ? "badge-active"
                                                                : "badge-inactive"
                                                        }
                                                    >

                                                        {department.isActive
                                                            ? "Active"
                                                            : "Inactive"}

                                                    </span>

                                                </td>

                                                <td>
                                                    {formatDate(department.createdAt)}
                                                </td>

                                                <td>

                                                    <div className="action-buttons">

                                                        <button
                                                            className="action-btn view"
                                                            onClick={() => handleView(department)}
                                                            title="View"
                                                        >
                                                            <Eye size={16} />
                                                        </button>

                                                        <button
                                                            className="action-btn edit"
                                                            onClick={() => openEdit(department)}
                                                            title="Edit"
                                                        >
                                                            <Pencil size={16} />
                                                        </button>

                                                        <button
                                                            className={
                                                                department.isActive
                                                                    ? "action-btn deactivate"
                                                                    : "action-btn activate"
                                                            }
                                                            onClick={() => handleStatusChange(department)}
                                                            title={
                                                                department.isActive
                                                                    ? "Deactivate"
                                                                    : "Activate"
                                                            }
                                                        >
                                                            <Power size={16} />
                                                        </button>

                                                        <button
                                                            className="action-btn delete"
                                                            onClick={() => handleDelete(department)}
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