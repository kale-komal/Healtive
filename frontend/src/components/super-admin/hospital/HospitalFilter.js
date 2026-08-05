"use client";

import "./HospitalFilter.css";

export default function HospitalFilter({

    search,
    setSearch,

    status,
    setStatus,

}) {

    return (

        <div className="hospital-filter">

            <input
                type="text"
                className="form-control"
                placeholder="Search hospital..."
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

    );

}