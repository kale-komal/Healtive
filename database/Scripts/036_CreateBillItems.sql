-- =============================================
-- Script: 036_CreateBillItems.sql
-- =============================================

CREATE TABLE BillItems
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    BillId CHAR(36) NOT NULL,

    ItemName VARCHAR(200) NOT NULL,

    Quantity DECIMAL(10,2) NOT NULL DEFAULT 1,

    UnitPrice DECIMAL(12,2) NOT NULL,

    DiscountAmount DECIMAL(12,2) NOT NULL DEFAULT 0,

    TaxAmount DECIMAL(12,2) NOT NULL DEFAULT 0,

    TotalAmount DECIMAL(12,2) NOT NULL,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_BillItems_Bill
        FOREIGN KEY (BillId)
        REFERENCES Bills(Id)
);