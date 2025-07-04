using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class PatientTreatmentResponse
    {
        public int PatientTreatmentId { get; set; }
        public int PatientId { get; set; }
        public int RegimenId { get; set; }
        public int PrescribingDoctorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public string? RegimenAdjustments { get; set; }
        public int? BaselineCD4 { get; set; }
        public string? BaselineHivViralLoad { get; set; }
        public string? ActualDosage { get; set; }
        public TreatmentStatus Status { get; set; }
        public string? ReasonForChangeOrStop { get; set; }
    }
}
