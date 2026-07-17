-- =============================================
-- Script: 027_CreateAppointments.sql
-- Module: Appointment Management
-- =============================================

CREATE TABLE Appointments
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    AppointmentNumber VARCHAR(30) NOT NULL UNIQUE,

    HospitalId CHAR(36) NOT NULL,

    BranchId CHAR(36) NOT NULL,

    PatientId CHAR(36) NOT NULL,

    DoctorId CHAR(36) NOT NULL,

    DepartmentId CHAR(36) NOT NULL,

    AppointmentStatusId CHAR(36) NOT NULL,

    AppointmentDate DATE NOT NULL,

    AppointmentTime TIME NOT NULL,

    TokenNumber INT NULL,

    ConsultationType VARCHAR(30) NOT NULL,

    ReasonForVisit VARCHAR(500),

    Notes TEXT,

    IsFirstVisit BOOLEAN NOT NULL DEFAULT TRUE,

    CreatedByUserId CHAR(36) NOT NULL,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    UpdatedAt DATETIME NULL DEFAULT NULL
        ON UPDATE CURRENT_TIMESTAMP,

    CONSTRAINT FK_Appointments_Hospitals
        FOREIGN KEY (HospitalId) REFERENCES Hospitals(Id),

    CONSTRAINT FK_Appointments_Branches
        FOREIGN KEY (BranchId) REFERENCES Branches(Id),

    CONSTRAINT FK_Appointments_Patients
        FOREIGN KEY (PatientId) REFERENCES Patients(Id),

    CONSTRAINT FK_Appointments_Doctors
        FOREIGN KEY (DoctorId) REFERENCES Doctors(Id),

    CONSTRAINT FK_Appointments_Departments
        FOREIGN KEY (DepartmentId) REFERENCES Departments(Id),

    CONSTRAINT FK_Appointments_Status
        FOREIGN KEY (AppointmentStatusId) REFERENCES AppointmentStatuses(Id),

    CONSTRAINT FK_Appointments_Users
        FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
);