-- =============================================
-- Script: 012_CreateAuditLogs.sql
-- Module: Audit
-- Description: Stores all important system activities
-- =============================================

CREATE TABLE AuditLogs
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NULL,

    UserId CHAR(36) NOT NULL,

    Module VARCHAR(100) NOT NULL,

    Action VARCHAR(100) NOT NULL,

    EntityName VARCHAR(100) NOT NULL,

    EntityId CHAR(36) NOT NULL,

    OldValues TEXT NULL,

    NewValues TEXT NULL,

    IpAddress VARCHAR(50),

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_AuditLogs_Hospitals
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_AuditLogs_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
);