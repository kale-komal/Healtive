-- =============================================
-- Script: 013_CreateDepartments.sql
-- Module: Organization
-- Description: Creates the Departments table
-- =============================================

CREATE TABLE Departments
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    Name VARCHAR(150) NOT NULL,

    Code VARCHAR(50) NOT NULL,

    Description VARCHAR(300) NULL,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,

    CONSTRAINT FK_Departments_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT UQ_Departments_Hospital_Code
        UNIQUE (HospitalId, Code),

    CONSTRAINT UQ_Departments_Hospital_Name
        UNIQUE (HospitalId, Name)
);