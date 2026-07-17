-- =============================================
-- Script: 022_CreatePatientEmergencyContacts.sql
-- =============================================

CREATE TABLE PatientEmergencyContacts
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientId CHAR(36) NOT NULL,

    Name VARCHAR(150) NOT NULL,

    Relationship VARCHAR(100) NOT NULL,

    MobileNumber VARCHAR(20) NOT NULL,

    AlternateNumber VARCHAR(20),

    Address VARCHAR(300),

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_PatientEmergencyContacts_Patients
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id)
);