import "./SubscriptionOverview.css";

export default function SubscriptionOverview({ dashboard }) {

    return (

        <div className="subscription-overview">

            <div className="overview-item">

                <div>

                    <h4>Active</h4>

                    <span>Running subscriptions</span>

                </div>

                <strong>

                    {dashboard?.activeSubscriptions ?? 0}

                </strong>

            </div>

            <div className="overview-item">

                <div>

                    <h4>Trial</h4>

                    <span>Free Trial</span>

                </div>

                <strong>

                    {dashboard?.trialSubscriptions ?? 0}

                </strong>

            </div>

            <div className="overview-item">

                <div>

                    <h4>Expired</h4>

                    <span>Need Renewal</span>

                </div>

                <strong>

                    {dashboard?.expiredSubscriptions ?? 0}

                </strong>

            </div>

            <div className="overview-item">

                <div>

                    <h4>Expiring Soon</h4>

                    <span>Within 7 Days</span>

                </div>

                <strong>

                    {dashboard?.expiringIn7Days ?? 0}

                </strong>

            </div>

        </div>

    );

}