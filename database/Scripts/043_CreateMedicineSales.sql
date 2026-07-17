-- =============================================
-- Script: 043_CreateMedicineSales.sql
-- =============================================

CREATE TABLE MedicineSales
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    BillId CHAR(36) NOT NULL,

    MedicineId CHAR(36) NOT NULL,

    Quantity DECIMAL(10,2) NOT NULL,

    UnitPrice DECIMAL(10,2) NOT NULL,

    TotalAmount DECIMAL(10,2) NOT NULL,

    SoldAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_MedicineSales_Bill
        FOREIGN KEY (BillId)
        REFERENCES Bills(Id),

    CONSTRAINT FK_MedicineSales_Medicine
        FOREIGN KEY (MedicineId)
        REFERENCES Medicines(Id)
);