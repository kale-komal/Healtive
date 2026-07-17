-- =============================================
-- Script: 045_CreateLabTests.sql
-- =============================================

CREATE TABLE LabTests
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    TestCode VARCHAR(30) NOT NULL UNIQUE,

    CategoryId CHAR(36) NOT NULL,

    Name VARCHAR(200) NOT NULL,

    Description VARCHAR(500),

    Price DECIMAL(10,2) NOT NULL,

    NormalRange VARCHAR(200),

    Unit VARCHAR(50),

    IsActive BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_LabTests_Category
        FOREIGN KEY (CategoryId)
        REFERENCES LabCategories(Id)
);