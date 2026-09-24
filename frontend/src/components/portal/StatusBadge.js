import "./StatusBadge.css";

const STATUS_STYLES = {
    SCHEDULED: "status-badge-scheduled",
    CONFIRMED: "status-badge-confirmed",
    WAITING: "status-badge-waiting",
    CHECKED_IN: "status-badge-checked-in",
    CALLED: "status-badge-called",
    COMPLETED: "status-badge-completed",
    CANCELLED: "status-badge-cancelled",
    NO_SHOW: "status-badge-no-show",
};

const normalize = (status) =>
    String(status || "")
        .toUpperCase()
        .replace(/[^A-Z0-9]+/g, "_")
        .replace(/^_+|_+$/g, "");

export default function StatusBadge({ status }) {

    const code = normalize(status);

    const className = STATUS_STYLES[code] || "status-badge-default";

    return (
        <span className={`status-badge ${className}`}>
            {status || "—"}
        </span>
    );

}