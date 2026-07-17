-- =============================================
-- Script: 047_CreatePatientLabResults.sql
-- =============================================

CREATE TABLE PatientLabResults
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientLabOrderId CHAR(36) NOT NULL,

    ResultValue VARCHAR(300),

    ResultFileUrl VARCHAR(500),

    Remarks TEXT,

    PerformedByUserId CHAR(36),

    ResultDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_PatientLabResults_Order
        FOREIGN KEY (PatientLabOrderId)
        REFERENCES PatientLabOrders(Id),

    CONSTRAINT FK_PatientLabResults_User
        FOREIGN KEY (PerformedByUserId)
        REFERENCES Users(Id)
);