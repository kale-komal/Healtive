-- =============================================
-- Script: 025_CreatePatientQRCodes.sql
-- =============================================

CREATE TABLE PatientQRCodes
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientId CHAR(36) NOT NULL,

    QRToken CHAR(36) NOT NULL UNIQUE,

    QRImageUrl VARCHAR(500),

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    GeneratedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_PatientQRCodes_Patients
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id)
);