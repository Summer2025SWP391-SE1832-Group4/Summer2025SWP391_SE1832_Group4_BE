using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
