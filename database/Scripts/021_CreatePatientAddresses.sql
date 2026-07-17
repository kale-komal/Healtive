-- =============================================
-- Script: 021_CreatePatientAddresses.sql
-- =============================================

CREATE TABLE PatientAddresses
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientId CHAR(36) NOT NULL,

    AddressType VARCHAR(50) NOT NULL,

    AddressLine1 VARCHAR(300) NOT NULL,

    AddressLine2 VARCHAR(300) NULL,

    City VARCHAR(100) NOT NULL,

    State VARCHAR(100) NOT NULL,

    Country VARCHAR(100) NOT NULL,

    PostalCode VARCHAR(20),

    IsDefault BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_PatientAddresses_Patients
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id)
);