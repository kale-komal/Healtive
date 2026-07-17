-- =============================================
-- Script: 024_CreatePatientDocuments.sql
-- =============================================

CREATE TABLE PatientDocuments
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientId CHAR(36) NOT NULL,

    DocumentType VARCHAR(100) NOT NULL,

    FileName VARCHAR(300) NOT NULL,

    FileUrl VARCHAR(500) NOT NULL,

    UploadedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UploadedByUserId CHAR(36) NULL,

    CONSTRAINT FK_PatientDocuments_Patients
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_PatientDocuments_Users
        FOREIGN KEY (UploadedByUserId)
        REFERENCES Users(Id)
);