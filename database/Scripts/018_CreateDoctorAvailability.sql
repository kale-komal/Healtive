-- =============================================
-- Script: 018_CreateDoctorAvailability.sql
-- =============================================

CREATE TABLE DoctorAvailability
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    DoctorId CHAR(36) NOT NULL,

    DayOfWeek TINYINT NOT NULL,

    StartTime TIME NOT NULL,

    EndTime TIME NOT NULL,

    MaxAppointments INT NOT NULL DEFAULT 0,

    IsAvailable BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_DoctorAvailability_Doctors
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id)
);