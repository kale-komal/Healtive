-- =============================================
-- Script: 016_CreateDoctorDepartments.sql
-- =============================================

CREATE TABLE DoctorDepartments
(
    DoctorId CHAR(36) NOT NULL,

    DepartmentId CHAR(36) NOT NULL,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    PRIMARY KEY (DoctorId, DepartmentId),

    CONSTRAINT FK_DoctorDepartments_Doctors
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id),

    CONSTRAINT FK_DoctorDepartments_Departments
        FOREIGN KEY (DepartmentId)
        REFERENCES Departments(Id)
);