-- =============================================
-- Script: 054_SeedAppointmentQueueStatuses.sql
-- Module: Appointment Management - Queue
-- Description: Seeds the queue-related appointment statuses
--              (WAITING, CHECKED_IN, CALLED) if they do not already exist.
--              Existing statuses are left untouched.
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
SELECT
    UUID(),
    'Waiting',
    'WAITING',
    'Patient is waiting for check-in.',
    6,
    TRUE
WHERE NOT EXISTS
(
    SELECT 1
    FROM AppointmentStatuses
    WHERE Code = 'WAITING'
);

INSERT INTO AppointmentStatuses
(
    Id,
    Name,
    Code,
    Description,
    DisplayOrder,
    IsActive
)
SELECT
    UUID(),
    'Checked In',
    'CHECKED_IN',
    'Patient has checked in and is waiting for the doctor.',
    7,
    TRUE
WHERE NOT EXISTS
(
    SELECT 1
    FROM AppointmentStatuses
    WHERE Code = 'CHECKED_IN'
);

INSERT INTO AppointmentStatuses
(
    Id,
    Name,
    Code,
    Description,
    DisplayOrder,
    IsActive
)
SELECT
    UUID(),
    'Called',
    'CALLED',
    'Patient has been called into the consultation.',
    8,
    TRUE
WHERE NOT EXISTS
(
    SELECT 1
    FROM AppointmentStatuses
    WHERE Code = 'CALLED'
);