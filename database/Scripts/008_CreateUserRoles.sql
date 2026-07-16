-- =============================================
-- Script: 008_CreateUserRoles.sql
-- =============================================

CREATE TABLE UserRoles
(
    UserId CHAR(36) NOT NULL,

    RoleId CHAR(36) NOT NULL,

    AssignedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY(UserId, RoleId),

    CONSTRAINT FK_UserRoles_Users
        FOREIGN KEY(UserId)
        REFERENCES Users(Id),

    CONSTRAINT FK_UserRoles_Roles
        FOREIGN KEY(RoleId)
        REFERENCES Roles(Id)
);