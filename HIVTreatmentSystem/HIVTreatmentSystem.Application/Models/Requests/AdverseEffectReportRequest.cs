using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
