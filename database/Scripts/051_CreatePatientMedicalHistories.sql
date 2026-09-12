-- =============================================
-- Script: 051_CreatePatientMedicalHistories.sql
-- Module: Patient Medical History
-- Description: Stores long-term patient medical history
-- =============================================

CREATE TABLE PatientMedicalHistories
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientId CHAR(36) NOT NULL,

    HospitalId CHAR(36) NOT NULL,

    DoctorId CHAR(36) NOT NULL,

    MedicalCondition VARCHAR(500) NULL,

    Diagnosis VARCHAR(1000) NULL,

    Treatment TEXT NULL,

    Notes TEXT NULL,

    RecordedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL
        DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,

    CONSTRAINT FK_PatientMedicalHistories_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_PatientMedicalHistories_Hospital
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_PatientMedicalHistories_Doctor
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id)
);