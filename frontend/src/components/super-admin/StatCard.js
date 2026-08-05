import "./StatCard.css";

export default function StatCard({
    title,
    value,
    icon,
    color = "#2D7EF9",
    subtitle,
}) {

    return (

        <div className="stat-card">

            <div className="stat-top">

                <div>

                    <p>{title}</p>

                    <h2>{value}</h2>

                </div>

                <div
                    className="stat-icon"
                    style={{ background: color }}
                >
                    {icon}
                </div>

            </div>

            <span>{subtitle}</span>

        </div>

    );

}