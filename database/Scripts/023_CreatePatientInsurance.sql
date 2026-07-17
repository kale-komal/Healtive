-- =============================================
-- Script: 023_CreatePatientInsurance.sql
-- =============================================

CREATE TABLE PatientInsurance
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientId CHAR(36) NOT NULL,

    InsuranceCompany VARCHAR(200) NOT NULL,

    PolicyNumber VARCHAR(100) NOT NULL,

    PolicyHolderName VARCHAR(150),

    ValidFrom DATE,

    ValidTo DATE,

    CoverageAmount DECIMAL(12,2),

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_PatientInsurance_Patients
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id)
);