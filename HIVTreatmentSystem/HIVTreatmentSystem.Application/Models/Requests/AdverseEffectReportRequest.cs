using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class AdverseEffectReportRequest
    {
        public int PatientId { get; set; }
        public DateOnly DateOccurred { get; set; }
        public string Description { get; set; }
        public AdverseEffectSeverityEnum Severity { get; set; }
    }
}
