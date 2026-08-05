"use client";

import "./Pagination.css";

export default function Pagination({

    currentPage,
    totalPages,
    onPageChange,

}) {

    if (totalPages <= 1) return null;

    return (

        <div className="pagination-wrapper">

            <button

                className="btn btn-light"

                disabled={currentPage === 1}

                onClick={() => onPageChange(currentPage - 1)}

            >

                Previous

            </button>

            <span>

                Page {currentPage} of {totalPages}

            </span>

            <button

                className="btn btn-light"

                disabled={currentPage === totalPages}

                onClick={() => onPageChange(currentPage + 1)}

            >

                Next

            </button>

        </div>

    );

}