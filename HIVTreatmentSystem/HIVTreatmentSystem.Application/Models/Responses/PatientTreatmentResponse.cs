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

        // Nested relations if needed
        public StandardARVRegimenResponse? Regimen { get; set; }
    }
} 