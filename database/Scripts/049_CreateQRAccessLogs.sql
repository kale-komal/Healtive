-- =============================================
-- Script: 049_CreateQRAccessLogs.sql
-- =============================================

CREATE TABLE QRAccessLogs
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientId CHAR(36) NOT NULL,

    UserId CHAR(36) NOT NULL,

    Action VARCHAR(100) NOT NULL,

    AccessTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    Remarks VARCHAR(300),

    CONSTRAINT FK_QRAccessLogs_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_QRAccessLogs_User
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
);