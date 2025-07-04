using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Domain.Entities
{
    public class AdverseEffectReport
    {
        public int Id { get; set; }
        public int PatientId { get; set; }
        public Patient Patient { get; set; }
        public DateOnly DateOccurred { get; set; }
        public AdverseEffectReportStatusEnum Status { get; set; } = AdverseEffectReportStatusEnum.Pending;
        public string Description { get; set; }
        public AdverseEffectSeverityEnum Severity { get; set; }
        public DateTime CreatedAt { get; set; } = GetVietnamTime();

        public static DateTime GetVietnamTime()
        {
            var vnTz = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
            var vnNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vnTz);
            return DateTime.SpecifyKind(vnNow, DateTimeKind.Unspecified);
        }
    }

}




