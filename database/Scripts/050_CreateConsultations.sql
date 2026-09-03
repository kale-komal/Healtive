-- =============================================
-- Script: 050_CreateConsultations.sql
-- Module: Doctor Consultation
-- Description: Stores doctor consultation records
-- =============================================

CREATE TABLE Consultations
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    AppointmentId CHAR(36) NOT NULL,

    PatientId CHAR(36) NOT NULL,

    DoctorId CHAR(36) NOT NULL,

    ConsultationDate DATE NOT NULL,

    ChiefComplaint VARCHAR(1000) NULL,

    ClinicalNotes TEXT NULL,

    ExaminationNotes TEXT NULL,

    TreatmentNotes TEXT NULL,

    Advice TEXT NULL,

    IsCompleted BOOLEAN NOT NULL DEFAULT FALSE,

    CompletedAt DATETIME NULL,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_Consultations_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_Consultations_Appointments
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id),

    CONSTRAINT FK_Consultations_Patients
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_Consultations_Doctors
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id)
);