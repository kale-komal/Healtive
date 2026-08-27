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


-- =============================================
-- Seed: Default Appointment Statuses
-- =============================================

INSERT INTO AppointmentStatuses
(
    Id,
    Name,
    Code,
    Description,
    DisplayOrder,
    IsActive
)
VALUES
(
    UUID(),
    'Scheduled',
    'SCHEDULED',
    'Appointment has been scheduled.',
    1,
    TRUE
),
(
    UUID(),
    'Confirmed',
    'CONFIRMED',
    'Appointment has been confirmed.',
    2,
    TRUE
),
(
    UUID(),
    'Completed',
    'COMPLETED',
    'Appointment has been completed.',
    3,
    TRUE
),
(
    UUID(),
    'Cancelled',
    'CANCELLED',
    'Appointment has been cancelled.',
    4,
    TRUE
),
(
    UUID(),
    'No Show',
    'NO_SHOW',
    'Patient did not attend the appointment.',
    5,
    TRUE
);