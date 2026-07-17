-- =============================================
-- Script: 046_CreatePatientLabOrders.sql
-- =============================================

CREATE TABLE PatientLabOrders
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    OrderNumber VARCHAR(30) NOT NULL UNIQUE,

    PatientId CHAR(36) NOT NULL,

    AppointmentId CHAR(36),

    DoctorId CHAR(36) NOT NULL,

    LabTestId CHAR(36) NOT NULL,

    OrderDate DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    Status VARCHAR(30) NOT NULL,

    Remarks VARCHAR(500),

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_PatientLabOrders_Patient
        FOREIGN KEY (PatientId)
        REFERENCES Patients(Id),

    CONSTRAINT FK_PatientLabOrders_Appointment
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id),

    CONSTRAINT FK_PatientLabOrders_Doctor
        FOREIGN KEY (DoctorId)
        REFERENCES Doctors(Id),

    CONSTRAINT FK_PatientLabOrders_LabTest
        FOREIGN KEY (LabTestId)
        REFERENCES LabTests(Id)
);