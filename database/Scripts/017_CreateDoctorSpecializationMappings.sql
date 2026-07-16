-- =============================================
-- Script: 017_CreateDoctorSpecializationMappings.sql
-- =============================================

CREATE TABLE DoctorSpecializationMappings
(
    DoctorId CHAR(36) NOT NULL,

    SpecializationId CHAR(36) NOT NULL,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY (DoctorId, SpecializationId),

    CONSTRAINT FK_DoctorSpecializationMappings_Doctors
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id),

    CONSTRAINT FK_DoctorSpecializationMappings_Specializations
        FOREIGN KEY (SpecializationId)
        REFERENCES DoctorSpecializations(Id)
);