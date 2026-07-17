-- =============================================
-- Script: 041_CreateMedicineStock.sql
-- =============================================

CREATE TABLE MedicineStock
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    MedicineId CHAR(36) NOT NULL,

    BranchId CHAR(36) NOT NULL,

    BatchNumber VARCHAR(100),

    ExpiryDate DATE,

    Quantity DECIMAL(10,2) NOT NULL,

    PurchasePrice DECIMAL(10,2),

    SellingPrice DECIMAL(10,2),

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_MedicineStock_Medicine
        FOREIGN KEY (MedicineId)
        REFERENCES Medicines(Id),

    CONSTRAINT FK_MedicineStock_Branch
        FOREIGN KEY (BranchId)
        REFERENCES Branches(Id)
);