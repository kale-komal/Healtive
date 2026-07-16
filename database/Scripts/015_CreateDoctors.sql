-- =============================================
-- Script: 015_CreateDoctors.sql
-- Module: Organization
-- Description: Creates the Doctors table
-- =============================================

CREATE TABLE Doctors
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    UserId CHAR(36) NOT NULL,

    DoctorCode VARCHAR(50) NOT NULL,

    RegistrationNumber VARCHAR(100) NOT NULL,

    Qualification VARCHAR(200) NOT NULL,

    ExperienceYears INT NOT NULL DEFAULT 0,

    ConsultationFee DECIMAL(10,2) NOT NULL DEFAULT 0,

    Gender VARCHAR(20) NOT NULL,

    DateOfBirth DATE NULL,

    JoiningDate DATE NULL,

    Bio TEXT NULL,

    ProfileImageUrl VARCHAR(300) NULL,

    IsAvailable BOOLEAN NOT NULL DEFAULT TRUE,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,

    CONSTRAINT FK_Doctors_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_Doctors_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id),

    CONSTRAINT UQ_Doctors_Code
        UNIQUE (HospitalId, DoctorCode),

    CONSTRAINT UQ_Doctors_Registration
        UNIQUE (RegistrationNumber)
);