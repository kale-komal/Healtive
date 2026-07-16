-- =============================================
-- Script: 005_CreateUsers.sql
-- Module: Authentication & Authorization
-- Description: Creates the Users table
-- =============================================

CREATE TABLE Users
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    BranchId CHAR(36) NULL,

    EmployeeCode VARCHAR(50) NULL,

    FirstName VARCHAR(100) NOT NULL,

    LastName VARCHAR(100) NOT NULL,

    Email VARCHAR(150) NOT NULL,

    MobileNumber VARCHAR(20) NOT NULL,

    PasswordHash VARCHAR(500) NOT NULL,

    ProfileImageUrl VARCHAR(300) NULL,

    IsEmailVerified BOOLEAN NOT NULL DEFAULT FALSE,

    IsMobileVerified BOOLEAN NOT NULL DEFAULT FALSE,

    LastLoginAt DATETIME NULL,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    IsDeleted BOOLEAN NOT NULL DEFAULT FALSE,

    CONSTRAINT FK_Users_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_Users_Branches
        FOREIGN KEY (BranchId)
        REFERENCES Branches(Id),

    CONSTRAINT UQ_Users_Email UNIQUE (Email),

    CONSTRAINT UQ_Users_Mobile UNIQUE (MobileNumber)
);