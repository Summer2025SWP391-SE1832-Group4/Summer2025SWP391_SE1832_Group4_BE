using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
