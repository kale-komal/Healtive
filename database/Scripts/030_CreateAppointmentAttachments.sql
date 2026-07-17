-- =============================================
-- Script: 030_CreateAppointmentAttachments.sql
-- =============================================

CREATE TABLE AppointmentAttachments
(
    Id CHAR(36) NOT NULL PRIMARY KEY,

    AppointmentId CHAR(36) NOT NULL,

    FileName VARCHAR(300) NOT NULL,

    FileUrl VARCHAR(500) NOT NULL,

    FileType VARCHAR(100),

    UploadedByUserId CHAR(36) NOT NULL,

    UploadedAt DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,

    CONSTRAINT FK_AppointmentAttachments_Appointment
        FOREIGN KEY (AppointmentId)
        REFERENCES Appointments(Id),

    CONSTRAINT FK_AppointmentAttachments_User
        FOREIGN KEY (UploadedByUserId)
        REFERENCES Users(Id)
);