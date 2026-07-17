-- =============================================
-- Script: 035_CreateBills.sql
-- Module: Billing
-- =============================================

CREATE TABLE Bills
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    BillNumber VARCHAR(30) NOT NULL UNIQUE,

    HospitalId CHAR(36) NOT NULL,

    BranchId CHAR(36) NOT NULL,

    PatientId CHAR(36) NOT NULL,

    AppointmentId CHAR(36) NULL,

    BillDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    SubTotal DECIMAL(12,2) NOT NULL DEFAULT 0,

    DiscountAmount DECIMAL(12,2) NOT NULL DEFAULT 0,

    TaxAmount DECIMAL(12,2) NOT NULL DEFAULT 0,

    TotalAmount DECIMAL(12,2) NOT NULL DEFAULT 0,

    PaidAmount DECIMAL(12,2) NOT NULL DEFAULT 0,

    BalanceAmount DECIMAL(12,2) NOT NULL DEFAULT 0,

    BillStatus VARCHAR(30) NOT NULL,

    Remarks VARCHAR(500),

    CreatedByUserId CHAR(36) NOT NULL,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_Bills_Hospital
        FOREIGN KEY (HospitalId)
        REFERENCES Hospitals(Id),

    CONSTRAINT FK_Bills_Branch
        FOREIGN KEY (BranchId)
        REFERENCES Branches(Id),

    CONSTRAINT FK_Bills_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_Bills_Appointment
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id),

    CONSTRAINT FK_Bills_User
        FOREIGN KEY (CreatedByUserId)
        REFERENCES Users(Id)
);