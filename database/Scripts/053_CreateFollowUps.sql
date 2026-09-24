-- =============================================
-- Script: 053_CreateFollowUps.sql
-- Module: Doctor Follow-up
-- =============================================

CREATE TABLE FollowUps
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    BranchId CHAR(36) NOT NULL,

    AppointmentId CHAR(36) NOT NULL,

    PatientId CHAR(36) NOT NULL,

    DoctorId CHAR(36) NOT NULL,

    FollowUpDate DATE NOT NULL,

    FollowUpNotes TEXT NULL,

    Status VARCHAR(30) NOT NULL DEFAULT 'Pending',

    IsCompleted BOOLEAN NOT NULL DEFAULT FALSE,

    CompletedAt DATETIME NULL,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL
        DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_FollowUps_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_FollowUps_Branches
        FOREIGN KEY (BranchId)
        REFERENCES Branches(Id),

    CONSTRAINT FK_FollowUps_Appointments
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id),

    CONSTRAINT FK_FollowUps_Patients
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_FollowUps_Doctors
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id),

    INDEX IX_FollowUps_Hospital_Doctor_Date_Status
        (HospitalId, DoctorId, FollowUpDate, Status)
);