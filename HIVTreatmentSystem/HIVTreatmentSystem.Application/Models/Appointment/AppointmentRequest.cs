using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class AppointmentRequest
    {
        public int PatientId { get; set; }
        public int DoctorId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public string AppointmentType { get; set; }
        public AppointmentStatus? Status { get; set; } = AppointmentStatus.Scheduled;
        public string ReasonForVisit { get; set; }
        public string AppointmentNotes { get; set; }
        public bool IsAnonymousConsultation { get; set; }
        public int CreatedByUserId { get; set; }
    }
}
