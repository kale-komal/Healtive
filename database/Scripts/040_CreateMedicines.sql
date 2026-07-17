-- =============================================
-- Script: 040_CreateMedicines.sql
-- =============================================

CREATE TABLE Medicines
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    MedicineCode VARCHAR(30) NOT NULL UNIQUE,

    CategoryId CHAR(36) NOT NULL,

    Name VARCHAR(200) NOT NULL,

    GenericName VARCHAR(200),

    BrandName VARCHAR(200),

    Strength VARCHAR(100),

    Manufacturer VARCHAR(200),

    Unit VARCHAR(50),

    MRP DECIMAL(10,2),

    SellingPrice DECIMAL(10,2),

    IsPrescriptionRequired BOOLEAN NOT NULL DEFAULT TRUE,

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_Medicines_Category
        FOREIGN KEY (CategoryId)
        REFERENCES MedicineCategories(Id)
);