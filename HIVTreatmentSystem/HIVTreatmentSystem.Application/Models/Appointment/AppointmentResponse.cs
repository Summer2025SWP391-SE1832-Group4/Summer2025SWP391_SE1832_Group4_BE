using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class AppointmentResponse
    {
        public int AppointmentId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public string? AppointmentType { get; set; }
        public AppointmentStatus Status { get; set; }
        public string? ReasonForVisit { get; set; }
        public bool IsAnonymousConsultation { get; set; }

        public AccountResponse? CreatedByUser { get; set; }
        public string? PatientName { get; set; }

        public virtual PatientResponse Patient { get; set; } = null!;
        public string? DoctorName { get; set; }

        public virtual DoctorResponse? Doctor { get; set; }
    }
}
