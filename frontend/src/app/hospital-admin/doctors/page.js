"use client";

import { useCallback, useEffect, useState } from "react";

import {
    Plus,
    Pencil,
    Eye,
    Power,
    Trash2,
    KeyRound,
    Stethoscope,
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

import "./Doctors.css";

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

const formatCurrency = (value) => {

    if (value === null || value === undefined || value === "") return "—";

    const amount = Number(value);

    if (Number.isNaN(amount)) return "—";

    return new Intl.NumberFormat("en-IN", {
        style: "currency",
        currency: "INR",
        maximumFractionDigits: 0,
    }).format(amount);

};

const toDoctorForm = (doctor) => ({

    fullName: doctor?.fullName || "",
    doctorCode: doctor?.doctorCode || "",
    registrationNumber: doctor?.registrationNumber || "",
    qualification: doctor?.qualification || "",
    gender: doctor?.gender || "",
    experienceYears: doctor?.experienceYears ?? "",
    consultationFee: doctor?.consultationFee ?? "",
    dateOfBirth: doctor?.dateOfBirth ? doctor.dateOfBirth.slice(0, 10) : "",
    joiningDate: doctor?.joiningDate ? doctor.joiningDate.slice(0, 10) : "",
    bio: doctor?.bio || "",
    email: doctor?.email || "",
    mobileNumber: doctor?.mobileNumber || "",
    branchId: doctor?.branchId || "",
    departmentId: doctor?.departmentId || "",

});

const buildPayload = (form) => ({

    fullName: form.fullName.trim(),
    registrationNumber: form.registrationNumber.trim(),
    qualification: form.qualification.trim(),
    gender: form.gender,
    experienceYears: Number(form.experienceYears) || 0,
    consultationFee: Number(form.consultationFee) || 0,
    dateOfBirth: form.dateOfBirth || null,
    joiningDate: form.joiningDate || null,
    bio: form.bio.trim() || null,
    email: form.email.trim(),
    mobileNumber: form.mobileNumber.trim(),
    branchId: form.branchId || null,
    departmentId: form.departmentId || null,

});

const buildViewHtml = (data) => {

    const statusLabel = data.isActive ? "Active" : "Inactive";

    const availableLabel = data.isAvailable ? "Available" : "Not Available";

    return `
        <div class="doctors-view">
            <div class="doctors-view-grid">
                <div class="doctors-view-item">
                    <span>Doctor Code</span>
                    <strong>${escapeHtml(data.doctorCode || "—")}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Full Name</span>
                    <strong>${escapeHtml(data.fullName || "—")}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Department</span>
                    <strong>${escapeHtml(data.departmentName || "—")}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Branch</span>
                    <strong>${escapeHtml(data.branchName || "—")}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Registration Number</span>
                    <strong>${escapeHtml(data.registrationNumber || "—")}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Qualification</span>
                    <strong>${escapeHtml(data.qualification || "—")}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Experience</span>
                    <strong>${escapeHtml(
                        data.experienceYears === null || data.experienceYears === undefined
                            ? "—"
                            : `${data.experienceYears} year(s)`
                    )}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Consultation Fee</span>
                    <strong>${escapeHtml(formatCurrency(data.consultationFee))}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Gender</span>
                    <strong>${escapeHtml(data.gender || "—")}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Mobile Number</span>
                    <strong>${escapeHtml(data.mobileNumber || "—")}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Email</span>
                    <strong>${escapeHtml(data.email || "—")}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Date of Birth</span>
                    <strong>${escapeHtml(formatDate(data.dateOfBirth))}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Joining Date</span>
                    <strong>${escapeHtml(formatDate(data.joiningDate))}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Availability</span>
                    <strong>${availableLabel}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Status</span>
                    <strong>${statusLabel}</strong>
                </div>
                <div class="doctors-view-item">
                    <span>Created At</span>
                    <strong>${escapeHtml(formatDate(data.createdAt))}</strong>
                </div>
                ${
                    data.bio
                        ? `
                    <div class="doctors-view-item doctors-view-item-wide">
                        <span>Bio</span>
                        <strong>${escapeHtml(data.bio)}</strong>
                    </div>`
                        : ""
                }
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

        <div className="doctors-searchable">

            <label className="form-label">
                {label}
            </label>

            <button
                type="button"
                className="form-select doctors-select-toggle"
                onClick={() => setOpen((prev) => !prev)}
            >

                <span>
                    {selected ? selected.name : placeholder}
                </span>

                <ChevronDown size={16} />

            </button>

            {open && (

                <div className="doctors-select-pop">

                    <input
                        autoFocus
                        type="text"
                        className="form-control doctors-select-search"
                        placeholder="Type to search..."
                        value={filter}
                        onChange={(e) => setFilter(e.target.value)}
                    />

                    <ul className="doctors-select-list">

                        {filtered.length === 0 ? (

                            <li className="doctors-select-empty">
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

export default function DoctorManagementPage() {

    const [mode, setMode] = useState("list");

    const [doctors, setDoctors] = useState([]);

    const [totalPages, setTotalPages] = useState(1);

    const [totalCount, setTotalCount] = useState(0);

    const [page, setPage] = useState(1);

    const [pageSize] = useState(10);

    const [search, setSearch] = useState("");

    const [status, setStatus] = useState("");

    const [available, setAvailable] = useState("");

    const [query, setQuery] = useState({
        search: "",
        status: "",
        available: "",
    });

    const [loading, setLoading] = useState(true);

    const [saving, setSaving] = useState(false);

    const [opening, setOpening] = useState(false);

    const [editingDoctorId, setEditingDoctorId] = useState(null);

    const [form, setForm] = useState(toDoctorForm(null));

    const [existingDoctorCodes, setExistingDoctorCodes] = useState([]);

    const [branches, setBranches] = useState([]);

    const [departments, setDepartments] = useState([]);

    const loadLookups = useCallback(async () => {

        try {

            const [branchResponse, departmentResponse] = await Promise.all([
                hospitalAdminService.getBranches({
                    page: 1,
                    pageSize: 100,
                }),
                hospitalAdminService.getDepartments({
                    page: 1,
                    pageSize: 100,
                }),
            ]);

            const branchNormalized = normalizeResponse(branchResponse);

            if (branchNormalized.ok) {

                const paged = branchNormalized.data;

                setBranches(
                    Array.isArray(paged?.items)
                        ? paged.items
                            .filter((branch) => branch.isActive)
                            .map((branch) => ({
                                id: branch.branchId,
                                name: branch.name,
                            }))
                        : []
                );

            }
            else {

                toast.error(
                    branchNormalized.message ||
                        "Failed to load branches."
                );

            }

            const departmentNormalized =
                normalizeResponse(departmentResponse);

            if (departmentNormalized.ok) {

                const paged = departmentNormalized.data;

                setDepartments(
                    Array.isArray(paged?.items)
                        ? paged.items
                            .filter((department) => department.isActive)
                            .map((department) => ({
                                id: department.departmentId,
                                name: department.name,
                            }))
                        : []
                );

            }
            else {

                toast.error(
                    departmentNormalized.message ||
                        "Failed to load departments."
                );

            }

        }
        catch (error) {

            console.error(error);

        }

    }, []);

    useEffect(() => {

        const timer = setTimeout(loadLookups, 0);

        return () => clearTimeout(timer);

    }, [loadLookups]);

    useEffect(() => {

        const timer = setTimeout(() => {

            setPage(1);

            setQuery((prev) => {

                const next = {
                    search: search.trim(),
                    status,
                    available,
                };

                if (
                    prev.search === next.search &&
                    prev.status === next.status &&
                    prev.available === next.available
                ) {
                    return prev;
                }

                return next;

            });

        }, 400);

        return () => clearTimeout(timer);

    }, [search, status, available]);

    const loadDoctors = useCallback(async () => {

        setLoading(true);

        try {

            const params = { page, pageSize };

            if (query.search) {
                params.search = query.search;
            }

            if (query.status) {
                params.status = query.status;
            }

            if (query.available) {
                params.available = query.available;
            }

            const response = await hospitalAdminService.getDoctors(params);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to load doctors."
                );

                setDoctors([]);

                return;

            }

            const paged = normalized.data;

            setDoctors(
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
                    "Something went wrong while loading doctors."
            );

            setDoctors([]);

        }
        finally {

            setLoading(false);

        }

    }, [page, query, pageSize]);

    useEffect(() => {

        const timer = setTimeout(loadDoctors, 0);

        return () => clearTimeout(timer);

    }, [loadDoctors]);

    const openCreate = async () => {

        setEditingDoctorId(null);

        setForm(toDoctorForm(null));

        setMode("create");

        await loadLookups();

        try {

            // Fetch existing codes so the auto-generated preview
            // receives a unique suffix (RAHUL-PATIL, RAHUL-PATIL-2, ...).
            const response = await hospitalAdminService.getDoctors({
                page: 1,
                pageSize: 100,
            });

            const normalized = normalizeResponse(response);

            if (!normalized.ok) return;

            const paged = normalized.data;

            if (Array.isArray(paged?.items)) {

                setExistingDoctorCodes(
                    paged.items.map((doctor) => doctor.doctorCode)
                );

            }

        }
        catch (error) {

            console.error(error);

        }

    };

    const openEdit = async (doctor) => {

        setOpening(true);

        try {

            const response = await hospitalAdminService.getDoctorById(
                doctor.doctorId
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message || "Failed to load doctor details."
                );

                return;

            }

            setEditingDoctorId(doctor.doctorId);

            setForm(toDoctorForm(normalized.data));

            await loadLookups();

            setMode("edit");

        }
        catch (error) {

            console.error(error);

            toast.error(
                "Something went wrong while loading the doctor."
            );

        }
        finally {

            setOpening(false);

        }

    };

    const cancelForm = () => {

        setEditingDoctorId(null);

        setForm(toDoctorForm(null));

        setMode("list");

    };

    const handleChange = (e) => {

        const { name, type, checked, value } = e.target;

        if (name === "fullName" && mode === "create") {

            const baseCode = generateCodeFromName(value);

            setForm((prev) => ({
                ...prev,
                fullName: value,
                doctorCode: uniqueCode(baseCode, existingDoctorCodes),
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

        if (!form.fullName.trim()) {

            toast.error("Full name is required.");

            return;

        }

        if (!form.mobileNumber.trim()) {

            toast.error("Mobile number is required.");

            return;

        }

        if (!form.branchId) {

            toast.error("Please select a branch.");

            return;

        }

        if (!form.departmentId) {

            toast.error("Please select a department.");

            return;

        }

        if (!form.registrationNumber.trim()) {

            toast.error("Registration number is required.");

            return;

        }

        if (!form.qualification.trim()) {

            toast.error("Qualification is required.");

            return;

        }

        if (!form.gender) {

            toast.error("Gender is required.");

            return;

        }

        setSaving(true);

        try {

            const payload = buildPayload(form);

            const response = mode === "edit"
                ? await hospitalAdminService.updateDoctor(
                    editingDoctorId,
                    payload
                )
                : await hospitalAdminService.createDoctor(payload);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(
                    normalized.message ||
                        (mode === "edit"
                            ? "Failed to update doctor."
                            : "Failed to create doctor.")
                );

                return;

            }

            toast.success(
                normalized.message ||
                    (mode === "edit"
                        ? "Doctor updated successfully."
                        : "Doctor created successfully.")
            );

            cancelForm();

            loadDoctors();

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

    const handleStatusChange = async (doctor) => {

        const isActive = doctor.isActive;

        const result = await Swal.fire({

            title: isActive
                ? "Deactivate Doctor?"
                : "Activate Doctor?",

            text: isActive
                ? "The doctor will become inactive and unavailable."
                : "The doctor will become active and available again.",

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
                ? await hospitalAdminService.deactivateDoctor(doctor.doctorId)
                : await hospitalAdminService.activateDoctor(doctor.doctorId);

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(normalized.message || "Something went wrong.");

                return;

            }

            toast.success(
                normalized.message ||
                    (isActive
                        ? "Doctor deactivated successfully."
                        : "Doctor activated successfully.")
            );

            loadDoctors();

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };

    const handleResetPassword = async (doctor) => {

        const result = await Swal.fire({

            title: "Reset Password?",

            text: `Reset the login password for ${doctor.fullName} to the temporary password.`,

            icon: "warning",

            showCancelButton: true,

            confirmButtonText: "Yes, Reset",

            cancelButtonText: "Cancel",

            reverseButtons: true,

        });

        if (!result.isConfirmed) return;

        try {

            const response = await hospitalAdminService.resetDoctorPassword(
                doctor.doctorId
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(normalized.message || "Something went wrong.");

                return;

            }

            toast.success(
                normalized.message ||
                    "Doctor password reset successfully."
            );

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };

    const handleDelete = async (doctor) => {

        const result = await Swal.fire({

            title: "Delete Doctor?",

            text: "The doctor and their login access will be removed. Historical records that reference this doctor are preserved.",

            icon: "warning",

            showCancelButton: true,

            confirmButtonText: "Yes, Delete",

            cancelButtonText: "Cancel",

            reverseButtons: true,

        });

        if (!result.isConfirmed) return;

        try {

            const response = await hospitalAdminService.deleteDoctor(
                doctor.doctorId
            );

            const normalized = normalizeResponse(response);

            if (!normalized.ok) {

                toast.error(normalized.message || "Something went wrong.");

                return;

            }

            toast.success(
                normalized.message || "Doctor deleted successfully."
            );

            if (doctors.length === 1 && page > 1) {

                setPage(page - 1);

            }
            else {

                loadDoctors();

            }

        }
        catch (error) {

            console.error(error);

            toast.error("Something went wrong.");

        }

    };

    const handleView = (doctor) => {

        Swal.fire({

            title: doctor.fullName || "Doctor Details",

            html: buildViewHtml(doctor),

            icon: "info",

            confirmButtonText: "Close",

            width: "min(640px, calc(100vw - 40px))",

        });

        (async () => {

            try {

                const response = await hospitalAdminService.getDoctorById(
                    doctor.doctorId
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
                    title={mode === "edit" ? "Edit Doctor" : "Add Doctor"}
                    subtitle={
                        mode === "edit"
                            ? "Update the doctor details and save your changes."
                            : "Add a new doctor for your hospital."
                    }
                />

                {opening ? (

                    <div className="doctors-loading">
                        <Loader2 className="doctors-spin" size={22} />
                        <span>Loading doctor details...</span>
                    </div>

                ) : (

                    <div className="doctors-form-card">

                        <form onSubmit={handleSubmit}>

                            <div className="doctors-form-section">

                                <h5>Personal Information</h5>

                                <div className="row g-3">

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Full Name
                                        </label>

                                        <input
                                            autoFocus
                                            type="text"
                                            className="form-control"
                                            name="fullName"
                                            value={form.fullName}
                                            onChange={handleChange}
                                            placeholder="e.g. Dr Rahul Patil"
                                        />

                                    </div>

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Doctor Code
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control doctor-code-preview"
                                            value={form.doctorCode}
                                            readOnly
                                            disabled
                                            placeholder="Auto-generated code"
                                        />

                                        <small className="doctors-form-hint">
                                            {
                                                mode === "edit"
                                                    ? "This code is preserved from the existing record."
                                                    : "Auto-generated from the full name. A numeric suffix is added when the code is already in use."
                                            }
                                        </small>

                                    </div>

                                    <div className="col-md-4">

                                        <SearchableSelect
                                            label="Branch"
                                            placeholder="Select branch"
                                            options={branches}
                                            value={form.branchId}
                                            onChange={(id) =>
                                                setForm((prev) => ({
                                                    ...prev,
                                                    branchId: id,
                                                }))
                                            }
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <SearchableSelect
                                            label="Department"
                                            placeholder="Select department"
                                            options={departments}
                                            value={form.departmentId}
                                            onChange={(id) =>
                                                setForm((prev) => ({
                                                    ...prev,
                                                    departmentId: id,
                                                }))
                                            }
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Registration Number
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="registrationNumber"
                                            value={form.registrationNumber}
                                            onChange={handleChange}
                                            placeholder="Enter registration number"
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Qualification
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="qualification"
                                            value={form.qualification}
                                            onChange={handleChange}
                                            placeholder="e.g. MBBS, MD"
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Gender
                                        </label>

                                        <select
                                            className="form-select"
                                            name="gender"
                                            value={form.gender}
                                            onChange={handleChange}
                                        >

                                            <option value="">
                                                Select Gender
                                            </option>

                                            <option value="Male">
                                                Male
                                            </option>

                                            <option value="Female">
                                                Female
                                            </option>

                                            <option value="Other">
                                                Other
                                            </option>

                                        </select>

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Experience (Years)
                                        </label>

                                        <input
                                            type="number"
                                            min="0"
                                            className="form-control"
                                            name="experienceYears"
                                            value={form.experienceYears}
                                            onChange={handleChange}
                                            placeholder="Enter experience"
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Consultation Fee (₹)
                                        </label>

                                        <input
                                            type="number"
                                            min="0"
                                            step="0.01"
                                            className="form-control"
                                            name="consultationFee"
                                            value={form.consultationFee}
                                            onChange={handleChange}
                                            placeholder="Enter fee"
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Date of Birth
                                        </label>

                                        <input
                                            type="date"
                                            className="form-control"
                                            name="dateOfBirth"
                                            value={form.dateOfBirth}
                                            onChange={handleChange}
                                        />

                                    </div>

                                    <div className="col-md-4">

                                        <label className="form-label">
                                            Joining Date
                                        </label>

                                        <input
                                            type="date"
                                            className="form-control"
                                            name="joiningDate"
                                            value={form.joiningDate}
                                            onChange={handleChange}
                                        />

                                    </div>

                                    <div className="col-md-8">

                                        <label className="form-label">
                                            Bio
                                        </label>

                                        <textarea
                                            rows="2"
                                            className="form-control"
                                            name="bio"
                                            value={form.bio}
                                            onChange={handleChange}
                                            placeholder="Short introduction (optional)"
                                        />

                                    </div>

                                </div>

                            </div>

                            <div className="doctors-form-section">

                                <h5>Login Information</h5>

                                <div className="row g-3">

                                    <div className="col-md-6">

                                        <label className="form-label">
                                            Mobile Number
                                        </label>

                                        <input
                                            type="text"
                                            className="form-control"
                                            name="mobileNumber"
                                            value={form.mobileNumber}
                                            onChange={handleChange}
                                            placeholder="Mobile number is also the login username"
                                            maxLength={20}
                                        />

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
                                            placeholder="Enter email"
                                        />

                                    </div>

                                </div>

                            </div>

                            <div className="doctors-form-actions">

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
                                                        className="doctors-spin"
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
                                                            ? "Update Doctor"
                                                            : "Save Doctor"}
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
                title="Doctors"
                subtitle="Manage your hospital doctors and their availability."
                action={

                    <button
                        type="button"
                        className="btn btn-primary"
                        onClick={openCreate}
                    >

                        <Plus size={18} />

                        <span>Add Doctor</span>

                    </button>

                }
            />

            <div className="doctors-table-card">

                <div className="doctors-table-header">

                    <h3>Doctor List</h3>

                    <span className="doctors-count">
                        {totalCount} doctor{totalCount === 1 ? "" : "s"}
                    </span>

                </div>

                <div className="doctors-filter">

                    <input
                        type="text"
                        className="form-control"
                        placeholder="Search doctors..."
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

                    <select
                        className="form-select"
                        value={available}
                        onChange={(e) => setAvailable(e.target.value)}
                    >

                        <option value="">
                            All Availability
                        </option>

                        <option value="true">
                            Available
                        </option>

                        <option value="false">
                            Not Available
                        </option>

                    </select>

                </div>

                {
                    loading ? (

                        <div className="doctors-loading">
                            <Loader2 className="doctors-spin" size={22} />
                            <span>Loading doctors...</span>
                        </div>

                    ) : doctors.length === 0 ? (

                        <div className="doctors-empty">

                            <Stethoscope size={40} />

                            <h4>No doctors yet</h4>

                            <p>
                                Add your first doctor to get started.
                            </p>

                            <button
                                type="button"
                                className="btn btn-primary"
                                onClick={openCreate}
                            >

                                <Plus size={16} />

                                <span>Add Doctor</span>

                            </button>

                        </div>

                    ) : (

                        <div className="table-responsive">

                            <table className="table doctors-table">

                                <thead>

                                    <tr>

                                        <th>Code</th>
                                        <th>Name</th>
                                        <th>Department</th>
                                        <th>Branch</th>
                                        <th>Mobile</th>
                                        <th>Email</th>
                                        <th>Availability</th>
                                        <th>Status</th>
                                        <th>Created</th>
                                        <th width="240">Actions</th>

                                    </tr>

                                </thead>

                                <tbody>

                                    {
                                        doctors.map((doctor) => (

                                            <tr key={doctor.doctorId}>

                                                <td className="doctors-code">
                                                    {doctor.doctorCode}
                                                </td>

                                                <td>

                                                    <strong>
                                                        {doctor.fullName}
                                                    </strong>

                                                </td>

                                                <td>
                                                    {doctor.departmentName || "—"}
                                                </td>

                                                <td>
                                                    {doctor.branchName || "—"}
                                                </td>

                                                <td>
                                                    {doctor.mobileNumber || "—"}
                                                </td>

                                                <td>
                                                    {doctor.email || "—"}
                                                </td>

                                                <td>

                                                    <span
                                                        className={
                                                            doctor.isAvailable
                                                                ? "badge-available"
                                                                : "badge-unavailable"
                                                        }
                                                    >

                                                        {doctor.isAvailable
                                                            ? "Available"
                                                            : "Not Available"}

                                                    </span>

                                                </td>

                                                <td>

                                                    <span
                                                        className={
                                                            doctor.isActive
                                                                ? "badge-active"
                                                                : "badge-inactive"
                                                        }
                                                    >

                                                        {doctor.isActive
                                                            ? "Active"
                                                            : "Inactive"}

                                                    </span>

                                                </td>

                                                <td>
                                                    {formatDate(doctor.createdAt)}
                                                </td>

                                                <td>

                                                    <div className="action-buttons">

                                                        <button
                                                            className="action-btn view"
                                                            onClick={() => handleView(doctor)}
                                                            title="View"
                                                        >
                                                            <Eye size={16} />
                                                        </button>

                                                        <button
                                                            className="action-btn edit"
                                                            onClick={() => openEdit(doctor)}
                                                            title="Edit"
                                                        >
                                                            <Pencil size={16} />
                                                        </button>

                                                        <button
                                                            className={
                                                                doctor.isActive
                                                                    ? "action-btn deactivate"
                                                                    : "action-btn activate"
                                                            }
                                                            onClick={() => handleStatusChange(doctor)}
                                                            title={
                                                                doctor.isActive
                                                                    ? "Deactivate"
                                                                    : "Activate"
                                                            }
                                                        >
                                                            <Power size={16} />
                                                        </button>

                                                        <button
                                                            className="action-btn reset"
                                                            onClick={() => handleResetPassword(doctor)}
                                                            title="Reset Password"
                                                        >
                                                            <KeyRound size={16} />
                                                        </button>

                                                        <button
                                                            className="action-btn delete"
                                                            onClick={() => handleDelete(doctor)}
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