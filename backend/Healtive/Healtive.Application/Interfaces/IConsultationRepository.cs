using Healtive.Application.DTOs.Doctor.Consultation;
using Healtive.Core.Entities;

namespace Healtive.Application.Interfaces;

public interface IConsultationRepository
{
    // =========================================================
    // CREATE
    // =========================================================

    Task<Consultation?> GetAppointmentForConsultationAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId);

    Task<bool> ExistsByAppointmentIdAsync(
        Guid appointmentId);

    Task CreateAsync(
        Consultation consultation);

    // =========================================================
    // GET
    // =========================================================

    Task<ConsultationResponse?> GetByAppointmentIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid appointmentId);

    // =========================================================
    // UPDATE
    // =========================================================

    Task<Consultation?> GetEntityByIdAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid consultationId);

    Task UpdateAsync(
        Consultation consultation);

    // =========================================================
    // COMPLETE CONSULTATION
    // =========================================================

    Task CompleteAsync(
        Guid hospitalId,
        Guid doctorId,
        Guid consultationId);
}