-- =============================================
-- Script: 009_CreateRolePermissions.sql
-- =============================================

CREATE TABLE RolePermissions
(
    RoleId CHAR(36) NOT NULL,

    PermissionId CHAR(36) NOT NULL,

    PRIMARY KEY(RoleId, PermissionId),

    CONSTRAINT FK_RolePermissions_Roles
        FOREIGN KEY(RoleId)
        REFERENCES Roles(Id),

    CONSTRAINT FK_RolePermissions_Permissions
        FOREIGN KEY(PermissionId)
        REFERENCES Permissions(Id)
);