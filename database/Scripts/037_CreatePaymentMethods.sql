-- =============================================
-- Script: 037_CreatePaymentMethods.sql
-- =============================================

CREATE TABLE PaymentMethods
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    Name VARCHAR(100) NOT NULL,

    Code VARCHAR(50) NOT NULL,

    Description VARCHAR(300),

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    DisplayOrder INT NOT NULL DEFAULT 0,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UNIQUE(Name),

    UNIQUE(Code)
);