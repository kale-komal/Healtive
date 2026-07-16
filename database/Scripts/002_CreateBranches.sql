-- =============================================
-- Script: 002_CreateBranches.sql
-- Module: SaaS Management
-- Description: Creates the Branches table
-- =============================================

CREATE TABLE Branches
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    Name VARCHAR(200) NOT NULL,

    Code VARCHAR(50) NOT NULL,

    Email VARCHAR(150),

    PhoneNumber VARCHAR(20),

    Address VARCHAR(300) NOT NULL,

    City VARCHAR(100) NOT NULL,

    State VARCHAR(100) NOT NULL,

    Country VARCHAR(100) NOT NULL,

    PostalCode VARCHAR(20),

    IsHeadOffice BOOLEAN NOT NULL DEFAULT FALSE,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,

    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,

    CONSTRAINT UQ_Hospital_BranchCode
    UNIQUE (HospitalId, Code),

    CONSTRAINT FK_Branches_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id)
);