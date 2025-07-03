using System;
using System.Collections.Generic;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    /// <summary>
    /// Response model for Medical Record
    /// Mỗi Patient chỉ có một MedicalRecord duy nhất (1-to-1 relationship)
    /// </summary>
    public class MedicalRecordResponse
    {
        /// <summary>
        /// Unique identifier for the medical record
        /// </summary>
        public int MedicalRecordId { get; set; }

        /// <summary>
        /// ID of the patient (1-to-1 relationship)
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
        /// Trạng thái mang thai của bệnh nhân
        /// </summary>
        public PregnancyStatus PregnancyStatus { get; set; }

        /// <summary>
        /// Phụ mang thai ở tuần thứ... (chỉ áp dụng khi PregnancyStatus = Pregnant)
        /// </summary>
        public int PregnancyWeek { get; set; }

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
        public string? UnderlyingDisease { get; set; }

        /// <summary>
        /// Patient's drug allergy history
        /// </summary>
        public string? DrugAllergyHistory { get; set; }

        /// <summary>
        /// Patient details
        /// </summary>
        public PatientResponse? Patient { get; set; }

        /// <summary>
        /// Doctor details
        /// </summary>
        public DoctorResponse? Doctor { get; set; }

        /// <summary>
        /// Tất cả test results của Patient này
        /// </summary>
        public ICollection<TestResultResponse>? TestResults { get; set; }

        /// <summary>
        /// All patient treatments for this patient
        /// </summary>
        public ICollection<PatientTreatmentResponse> PatientTreatments { get; set; } = new List<PatientTreatmentResponse>();
    }
} 