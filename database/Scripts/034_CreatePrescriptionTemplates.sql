-- =============================================
-- Script: 034_CreatePrescriptionTemplates.sql
-- Module: Prescription Management
-- =============================================

CREATE TABLE PrescriptionTemplates
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    DoctorId CHAR(36) NOT NULL,

    Name VARCHAR(200) NOT NULL,

    Diagnosis VARCHAR(300),

    Advice TEXT,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_PrescriptionTemplates_Hospital
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_PrescriptionTemplates_Doctor
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id)
);