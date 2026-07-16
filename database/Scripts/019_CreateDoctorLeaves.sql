-- =============================================
-- Script: 019_CreateDoctorLeaves.sql
-- =============================================

CREATE TABLE DoctorLeaves
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    DoctorId CHAR(36) NOT NULL,

    FromDate DATE NOT NULL,

    ToDate DATE NOT NULL,

    Reason VARCHAR(300),

    IsApproved BOOLEAN NOT NULL DEFAULT FALSE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_DoctorLeaves_Doctors
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id)
);