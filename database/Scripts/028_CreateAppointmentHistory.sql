-- =============================================
-- Script: 028_CreateAppointmentHistory.sql
-- Module: Appointment Management
-- Description: Stores appointment status history
-- =============================================

CREATE TABLE AppointmentHistory
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    AppointmentId CHAR(36) NOT NULL,

    AppointmentStatusId CHAR(36) NOT NULL,

    ChangedByUserId CHAR(36) NOT NULL,

    Remarks VARCHAR(500),

    ChangedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_AppointmentHistory_Appointment
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id),

    CONSTRAINT FK_AppointmentHistory_Status
        FOREIGN KEY (AppointmentStatusId)
        REFERENCES AppointmentStatuses(Id),

    CONSTRAINT FK_AppointmentHistory_User
        FOREIGN KEY (ChangedByUserId)
        REFERENCES Users(Id)
);