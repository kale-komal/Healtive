-- =============================================
-- Script: 033_CreateMedicineDosages.sql
-- Module: Prescription Management
-- =============================================

CREATE TABLE MedicineDosages
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    Name VARCHAR(100) NOT NULL,

    Code VARCHAR(50) NOT NULL,

    Description VARCHAR(300),

    DisplayOrder INT NOT NULL DEFAULT 0,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT UQ_MedicineDosages_Name
        UNIQUE(Name),

    CONSTRAINT UQ_MedicineDosages_Code
        UNIQUE(Code)
);

-- =============================================
-- Seed: Default Medicine Dosages
-- =============================================


INSERT INTO MedicineDosages
(
    Id,
    Name,
    Code,
    Description,
    DisplayOrder,
    IsActive,
    CreatedAt
)
VALUES
(UUID(), 'Once Daily', 'OD', 'Take once a day', 1, 1, CURRENT_TIMESTAMP),
(UUID(), 'Twice Daily', 'BD', 'Take twice a day', 2, 1, CURRENT_TIMESTAMP),
(UUID(), 'Three Times Daily', 'TDS', 'Take three times a day', 3, 1, CURRENT_TIMESTAMP),
(UUID(), 'Four Times Daily', 'QID', 'Take four times a day', 4, 1, CURRENT_TIMESTAMP),
(UUID(), 'Every Morning', 'AM', 'Take in the morning', 5, 1, CURRENT_TIMESTAMP),
(UUID(), 'Every Night', 'PM', 'Take at night', 6, 1, CURRENT_TIMESTAMP),
(UUID(), 'At Bedtime', 'HS', 'Take at bedtime', 7, 1, CURRENT_TIMESTAMP),
(UUID(), 'As Needed', 'SOS', 'Take when required', 8, 1, CURRENT_TIMESTAMP),
(UUID(), 'Before Food', 'AC', 'Take before meals', 9, 1, CURRENT_TIMESTAMP),
(UUID(), 'After Food', 'PC', 'Take after meals', 10, 1, CURRENT_TIMESTAMP),
(UUID(), 'Empty Stomach', 'BBF', 'Take before breakfast on an empty stomach', 11, 1, CURRENT_TIMESTAMP);