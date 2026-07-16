-- =============================================
-- Script: 011_CreateUserLoginHistory.sql
-- Module: Authentication & Authorization
-- Description: Stores User Login History
-- =============================================

CREATE TABLE UserLoginHistory
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    UserId CHAR(36) NOT NULL,

    LoginTime DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    LogoutTime DATETIME NULL,

    IpAddress VARCHAR(50),

    Device VARCHAR(200),

    Browser VARCHAR(100),

    OperatingSystem VARCHAR(100),

    IsSuccessful BOOLEAN NOT NULL,

    CONSTRAINT FK_UserLoginHistory_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
);