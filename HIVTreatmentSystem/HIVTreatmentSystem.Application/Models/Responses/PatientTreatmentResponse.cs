namespace HIVTreatmentSystem.Application.Models.Responses
{
    /// <summary>
    /// Response model representing patient treatment information
    /// </summary>
    public class PatientTreatmentResponse
    {
        public int PatientTreatmentId { get; set; }
        public int PatientId { get; set; }
        public int RegimenId { get; set; }
        public int PrescribingDoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public string? RegimenAdjustments { get; set; }
        public string? ActualDosage { get; set; }
        public string Status { get; set; } = string.Empty;
        public string? ReasonForChangeOrStop { get; set; }

        // Nested relations
        public StandardARVRegimenResponse? Regimen { get; set; }
        public PatientResponse? Patient { get; set; }
        public DoctorResponse? PrescribingDoctor { get; set; }
        
        // Additional related data by Patient ID
        public ICollection<TestResultResponse>? PatientTestResults { get; set; }
        public MedicalRecordResponse? PatientMedicalRecord { get; set; }
    }
} 