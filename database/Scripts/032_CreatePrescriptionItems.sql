-- =============================================
-- Script: 032_CreatePrescriptionItems.sql
-- Module: Prescription Management
-- =============================================

CREATE TABLE PrescriptionItems
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PrescriptionId CHAR(36) NOT NULL,

    MedicineName VARCHAR(200) NOT NULL,

    DosageId CHAR(36) NOT NULL,

    Strength VARCHAR(100),

    Route VARCHAR(50),

    Frequency VARCHAR(100),

    DurationDays INT NOT NULL,

    Quantity DECIMAL(10,2) NOT NULL,

    Instructions VARCHAR(500),

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_PrescriptionItems_Prescription
        FOREIGN KEY (PrescriptionId)
        REFERENCES Prescriptions(Id),

    CONSTRAINT FK_PrescriptionItems_Dosage
        FOREIGN KEY (DosageId)
        REFERENCES MedicineDosages(Id)
);