-- =============================================
-- Script: 048_CreateQRScanHistory.sql
-- Module: QR System
-- =============================================

CREATE TABLE QRScanHistory
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PatientQRCodeId CHAR(36) NOT NULL,

    HospitalId CHAR(36) NOT NULL,

    BranchId CHAR(36) NOT NULL,

    ScannedByUserId CHAR(36) NOT NULL,

    ScanTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    DeviceInfo VARCHAR(300),

    IPAddress VARCHAR(100),

    Remarks VARCHAR(300),

    CONSTRAINT FK_QRScanHistory_QR
        FOREIGN KEY (PatientQRCodeId)
        REFERENCES PatientQRCodes(Id),

    CONSTRAINT FK_QRScanHistory_Hospital
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_QRScanHistory_Branch
        FOREIGN KEY (BranchId)
        REFERENCES Branches(Id),

    CONSTRAINT FK_QRScanHistory_User
        FOREIGN KEY (ScannedByUserId)
        REFERENCES Users(Id)
);