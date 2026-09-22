-- =============================================
-- Script: 051_CreateDiagnoses.sql
-- Module: Doctor / Diagnosis
-- =============================================

CREATE TABLE Diagnoses
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    AppointmentId CHAR(36) NOT NULL,

    PatientId CHAR(36) NOT NULL,

    HospitalId CHAR(36) NOT NULL,

    DoctorId CHAR(36) NOT NULL,

    DiagnosisName VARCHAR(500) NOT NULL,

    Description VARCHAR(1000) NULL,

    Severity VARCHAR(50) NULL,

    Notes TEXT NULL,

    DiagnosedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL
        DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_Diagnoses_Appointments
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id),

    CONSTRAINT FK_Diagnoses_Patients
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_Diagnoses_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_Diagnoses_Doctors
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id)
);