using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class AdverseEffectReportUpdateRequest
    {
        public DateOnly? DateOccurred { get; set; }
        public string? Description { get; set; }
        public AdverseEffectSeverityEnum? Severity { get; set; }
        public AdverseEffectReportStatusEnum? Status { get; set; }
    }
}
