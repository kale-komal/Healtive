-- =============================================
-- Script: 007_CreatePermissions.sql
-- Module: Authentication & Authorization
-- Description: Creates the Permissions table
-- =============================================

CREATE TABLE Permissions
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    Module VARCHAR(100) NOT NULL,

    Name VARCHAR(100) NOT NULL,

    Code VARCHAR(150) NOT NULL,

    Description VARCHAR(300),

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT UQ_Permission_Code
        UNIQUE(Code)
);