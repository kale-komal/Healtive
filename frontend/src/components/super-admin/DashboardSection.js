import "./DashboardSection.css";

export default function DashboardSection({
    title,
    children,
}) {

    return (

        <div className="dashboard-section">

            <div className="dashboard-section-header">

                <h3>{title}</h3>

            </div>

            <div className="dashboard-section-body">

                {children}

            </div>

        </div>

    );

}