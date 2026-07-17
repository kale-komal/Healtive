-- =============================================
-- Script: 031_CreatePrescriptions.sql
-- Module: Prescription Management
-- =============================================

CREATE TABLE Prescriptions
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PrescriptionNumber VARCHAR(30) NOT NULL UNIQUE,

    HospitalId CHAR(36) NOT NULL,

    BranchId CHAR(36) NOT NULL,

    AppointmentId CHAR(36) NOT NULL,

    PatientId CHAR(36) NOT NULL,

    DoctorId CHAR(36) NOT NULL,

    PrescriptionDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    Diagnosis VARCHAR(500),

    ClinicalNotes TEXT,

    Advice TEXT,

    FollowUpDate DATE NULL,

    IsFinalized BOOLEAN NOT NULL DEFAULT FALSE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_Prescriptions_Hospital
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_Prescriptions_Branch
        FOREIGN KEY (BranchId)
        REFERENCES Branches(Id),

    CONSTRAINT FK_Prescriptions_Appointment
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id),

    CONSTRAINT FK_Prescriptions_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_Prescriptions_Doctor
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id)
);