-- =============================================
-- Script: 050_CreateQRDevices.sql
-- =============================================

CREATE TABLE QRDevices
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    HospitalId CHAR(36) NOT NULL,

    BranchId CHAR(36) NOT NULL,

    DeviceName VARCHAR(150) NOT NULL,

    DeviceIdentifier VARCHAR(200),

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    RegisteredAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_QRDevices_Hospital
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_QRDevices_Branch
        FOREIGN KEY (BranchId)
        REFERENCES Branches(Id)
);