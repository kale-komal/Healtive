-- =============================================
-- Script: 003_CreateSubscriptionPlans.sql
-- Module: SaaS Management
-- Description: Creates the SubscriptionPlans table
-- =============================================

CREATE TABLE SubscriptionPlans
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    Name VARCHAR(100) NOT NULL,

    Description VARCHAR(500),

    Price DECIMAL(10,2) NOT NULL,

    DurationInDays INT NOT NULL,

    MaxBranches INT NOT NULL,

    MaxDoctors INT NOT NULL,

    MaxPatients INT NOT NULL,

    IsTrial BOOLEAN NOT NULL DEFAULT FALSE,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP
);