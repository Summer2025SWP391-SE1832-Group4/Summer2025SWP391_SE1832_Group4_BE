using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class AdverseEffectReportResponse
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public PatientResponse Patient { get; set; } = null!;
        public AdverseEffectReportStatusEnum Status { get; set; }
        public DateOnly DateOccurred { get; set; }
        public string Description { get; set; }
        public AdverseEffectSeverityEnum Severity { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
