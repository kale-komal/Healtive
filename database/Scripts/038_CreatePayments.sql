-- =============================================
-- Script: 038_CreatePayments.sql
-- =============================================

CREATE TABLE Payments
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    PaymentNumber VARCHAR(30) NOT NULL UNIQUE,

    BillId CHAR(36) NOT NULL,

    PaymentMethodId CHAR(36) NOT NULL,

    Amount DECIMAL(12,2) NOT NULL,

    TransactionReference VARCHAR(150),

    PaymentDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    ReceivedByUserId CHAR(36) NOT NULL,

    Remarks VARCHAR(300),

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_Payments_Bills
        FOREIGN KEY (BillId)
        REFERENCES Bills(Id),

    CONSTRAINT FK_Payments_Method
        FOREIGN KEY (PaymentMethodId)
        REFERENCES PaymentMethods(Id),

    CONSTRAINT FK_Payments_User
        FOREIGN KEY (ReceivedByUserId)
        REFERENCES Users(Id)
);