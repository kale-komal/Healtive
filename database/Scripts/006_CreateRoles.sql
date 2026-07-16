-- =============================================
-- Script: 006_CreateRoles.sql
-- Module: Authentication & Authorization
-- Description: Creates the Roles table
-- =============================================

CREATE TABLE Roles
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    Name VARCHAR(100) NOT NULL,

    Description VARCHAR(300),

    IsSystemRole BOOLEAN NOT NULL DEFAULT FALSE,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,

    CONSTRAINT FK_Roles_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT UQ_Hospital_Role
        UNIQUE(HospitalId, Name)
);