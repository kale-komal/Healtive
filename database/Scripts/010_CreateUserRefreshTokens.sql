-- =============================================
-- Script: 010_CreateUserRefreshTokens.sql
-- Module: Authentication & Authorization
-- Description: Stores JWT Refresh Tokens
-- =============================================

CREATE TABLE UserRefreshTokens
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    UserId CHAR(36) NOT NULL,

    RefreshToken VARCHAR(500) NOT NULL,

    ExpiresAt DATETIME NOT NULL,

    IsRevoked BOOLEAN NOT NULL DEFAULT FALSE,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    RevokedAt DATETIME NULL,

    CONSTRAINT FK_UserRefreshTokens_Users
        FOREIGN KEY (UserId)
        REFERENCES Users(Id)
);