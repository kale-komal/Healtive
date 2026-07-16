-- =============================================
-- Script: 020_CreatePatients.sql
-- Module: Patient Management
-- Description: Creates the Patients table
-- =============================================

CREATE TABLE Patients
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientCode VARCHAR(30) NOT NULL UNIQUE,

    FirstName VARCHAR(100) NOT NULL,

    LastName VARCHAR(100) NOT NULL,

    DateOfBirth DATE NULL,

    Gender VARCHAR(20) NOT NULL,

    BloodGroup VARCHAR(10) NULL,

    MobileNumber VARCHAR(20) NOT NULL UNIQUE,

    Email VARCHAR(150) NULL UNIQUE,

    PasswordHash VARCHAR(500) NULL,

    GoogleId VARCHAR(255) NULL,

    IsMobileVerified BOOLEAN NOT NULL DEFAULT FALSE,

    IsEmailVerified BOOLEAN NOT NULL DEFAULT FALSE,

    QRToken CHAR(36) NOT NULL UNIQUE,

    ProfileImageUrl VARCHAR(300) NULL,

    LastLoginAt DATETIME NULL,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE
);