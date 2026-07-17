-- =============================================
-- Script: 042_CreateMedicinePurchases.sql
-- =============================================

CREATE TABLE MedicinePurchases
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    MedicineId CHAR(36) NOT NULL,

    BranchId CHAR(36) NOT NULL,

    SupplierName VARCHAR(200),

    BatchNumber VARCHAR(100),

    Quantity DECIMAL(10,2) NOT NULL,

    PurchasePrice DECIMAL(10,2) NOT NULL,

    PurchaseDate DATE NOT NULL,

    ExpiryDate DATE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_MedicinePurchases_Medicine
        FOREIGN KEY (MedicineId)
        REFERENCES Medicines(Id),

    CONSTRAINT FK_MedicinePurchases_Branch
        FOREIGN KEY (BranchId)
        REFERENCES Branches(Id)
);