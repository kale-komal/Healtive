-- =============================================
-- Script: 004_CreateHospitalSubscriptions.sql
-- Module: SaaS Management
-- Description: Creates the HospitalSubscriptions table
-- =============================================

CREATE TABLE HospitalSubscriptions
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    SubscriptionPlanId CHAR(36) NOT NULL,

    StartDate DATETIME NOT NULL,

    EndDate DATETIME NOT NULL,

    TrialEndsOn DATETIME NULL,

    AmountPaid DECIMAL(10,2) NOT NULL DEFAULT 0,

    PaymentStatus VARCHAR(50) NOT NULL,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_HospitalSubscriptions_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_HospitalSubscriptions_SubscriptionPlans
        FOREIGN KEY (SubscriptionPlanId)
        REFERENCES SubscriptionPlans(Id)
);