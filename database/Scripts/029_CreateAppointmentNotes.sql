-- =============================================
-- Script: 029_CreateAppointmentNotes.sql
-- =============================================

CREATE TABLE AppointmentNotes
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    AppointmentId CHAR(36) NOT NULL,

    Note TEXT NOT NULL,

    CreatedByUserId CHAR(36) NOT NULL,

    CreatedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_AppointmentNotes_Appointment
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id),

    CONSTRAINT FK_AppointmentNotes_User
        FOREIGN KEY (CreatedByUserId)
        REFERENCES Users(Id)
);