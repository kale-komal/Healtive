-- =============================================
-- Script: 026_CreateAppointmentStatuses.sql
-- Module: Appointment Management
-- =============================================

CREATE TABLE AppointmentStatuses
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    Name VARCHAR(100) NOT NULL,

    Code VARCHAR(50) NOT NULL,

    Description VARCHAR(300),

    DisplayOrder INT NOT NULL DEFAULT 0,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT UQ_AppointmentStatuses_Name
        UNIQUE(Name),

    CONSTRAINT UQ_AppointmentStatuses_Code
        UNIQUE(Code)
);