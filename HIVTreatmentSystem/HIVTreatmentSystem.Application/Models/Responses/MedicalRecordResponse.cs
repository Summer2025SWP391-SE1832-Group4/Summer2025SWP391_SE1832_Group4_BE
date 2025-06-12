using System;
using System.Collections.Generic;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    /// <summary>
    /// Response model for Medical Record
    /// </summary>
    public class MedicalRecordResponse
    {
        /// <summary>
        /// Unique identifier for the medical record
        /// </summary>
        public int MedicalRecordId { get; set; }

        /// <summary>
        /// ID of the associated appointment
        /// </summary>
        public int AppointmentId { get; set; }

        /// <summary>
        /// ID of the patient
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// ID of the doctor
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Date of the consultation
        /// </summary>
        public DateTime ConsultationDate { get; set; }

        /// <summary>
        /// Patient's reported symptoms
        /// </summary>
        public string? Symptoms { get; set; }

        /// <summary>
        /// Doctor's diagnosis
        /// </summary>
        public string? Diagnosis { get; set; }

        /// <summary>
        /// Additional notes from the doctor
        /// </summary>
        public string? DoctorNotes { get; set; }

        /// <summary>
        /// Recommended next steps
        /// </summary>
        public string? NextSteps { get; set; }

        /// <summary>
        /// Any co-infection diseases
        /// </summary>
        public string? CoinfectionDiseases { get; set; }

        /// <summary>
        /// Patient's drug allergy history
        /// </summary>
        public string? DrugAllergyHistory { get; set; }

        /// <summary>
        /// Associated appointment details
        /// </summary>
        public AppointmentResponse? Appointment { get; set; }

        /// <summary>
        /// Patient details
        /// </summary>
        public PatientResponse? Patient { get; set; }

        /// <summary>
        /// Doctor details
        /// </summary>
        public DoctorResponse? Doctor { get; set; }

        /// <summary>
        /// Associated test results
        /// </summary>
        public ICollection<TestResultResponse>? TestResults { get; set; }
    }
} 