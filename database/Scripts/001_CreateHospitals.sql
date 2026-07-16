-- =============================================
-- Script: 001_CreateHospitals.sql
-- Module: SaaS Management
-- Description: Creates the Hospitals table
-- =============================================

CREATE TABLE Hospitals
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    Name VARCHAR(200) NOT NULL,

    Code VARCHAR(50) NOT NULL UNIQUE,

    LicenseNumber VARCHAR(100),

    GSTNumber VARCHAR(50),

    HospitalType VARCHAR(100) NOT NULL,

    Email VARCHAR(150) NOT NULL,

    PhoneNumber VARCHAR(20) NOT NULL,

    Website VARCHAR(200),

    LogoUrl VARCHAR(300),

    Address VARCHAR(300) NOT NULL,

    City VARCHAR(100) NOT NULL,

    State VARCHAR(100) NOT NULL,

    Country VARCHAR(100) NOT NULL,

    PostalCode VARCHAR(20),

    TimeZone VARCHAR(100) NOT NULL,

    Currency VARCHAR(20) NOT NULL,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,

    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE
);