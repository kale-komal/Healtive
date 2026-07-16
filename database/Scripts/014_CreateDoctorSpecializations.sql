-- =============================================
-- Script: 014_CreateDoctorSpecializations.sql
-- Module: Organization
-- Description: Creates the DoctorSpecializations table
-- =============================================

CREATE TABLE DoctorSpecializations
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    Name VARCHAR(150) NOT NULL,

    Code VARCHAR(50) NOT NULL,

    Description VARCHAR(300) NULL,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,

    CONSTRAINT UQ_DoctorSpecializations_Name
        UNIQUE(Name),

    CONSTRAINT UQ_DoctorSpecializations_Code
        UNIQUE(Code)
);