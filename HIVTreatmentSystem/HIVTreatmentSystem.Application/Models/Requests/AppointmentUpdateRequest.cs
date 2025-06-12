using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class AppointmentUpdateRequest
    {
        public DateOnly? AppointmentDate { get; set; }
        public TimeOnly? AppointmentTime { get; set; }
        public AppointmentTypeEnum? AppointmentType { get; set; }
        public AppointmentStatus? Status { get; set; }
        public AppointmentServiceEnum? AppointmentService { get; set; }
        public string? AppointmentNotes { get; set; }
    }
}
