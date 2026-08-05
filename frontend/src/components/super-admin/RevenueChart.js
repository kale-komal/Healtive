"use client";

import {
    ResponsiveContainer,
    AreaChart,
    Area,
    XAxis,
    Tooltip,
} from "recharts";

import "./RevenueChart.css";

const data = [
    { month: "Jan", revenue: 12000 },
    { month: "Feb", revenue: 18000 },
    { month: "Mar", revenue: 16000 },
    { month: "Apr", revenue: 25000 },
    { month: "May", revenue: 30000 },
    { month: "Jun", revenue: 28000 },
];

export default function RevenueChart() {

    return (

        <div className="revenue-chart">

            <ResponsiveContainer
                width="100%"
                height={280}
            >

                <AreaChart data={data}>

                    <XAxis dataKey="month" />

                    <Tooltip />

                    <Area
                        type="monotone"
                        dataKey="revenue"
                        stroke="#2D7EF9"
                        fill="#EAF3FF"
                    />

                </AreaChart>

            </ResponsiveContainer>

        </div>

    );

}