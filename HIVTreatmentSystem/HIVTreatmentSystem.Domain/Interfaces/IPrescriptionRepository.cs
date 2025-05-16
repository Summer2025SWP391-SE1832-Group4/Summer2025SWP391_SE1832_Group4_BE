using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IPrescriptionRepository : IGenericRepository<Prescription>
    {
        Task<Prescription> GetPrescriptionWithDetailsAsync(Guid id);
        Task<IEnumerable<Prescription>> GetPrescriptionsByPatientAsync(Guid patientId);
        Task<IEnumerable<Prescription>> GetPrescriptionsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<Prescription>> GetPrescriptionsByTreatmentAsync(Guid treatmentId);
        Task<IEnumerable<Prescription>> GetActivePrescriptionsAsync();
        Task<IEnumerable<Prescription>> GetPrescriptionsByMedicationAsync(string medicationName);
    }
} 