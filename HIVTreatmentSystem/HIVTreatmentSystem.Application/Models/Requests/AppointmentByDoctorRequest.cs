using HIVTreatmentSystem.Domain.Enums;

using System.ComponentModel.DataAnnotations;


namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class AppointmentByDoctorRequest
    {
        public int DoctorId { get; set; }
        public int PatientId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        [EnumDataType(typeof(AppointmentTypeEnum))]
        public AppointmentTypeEnum AppointmentType { get; set; }
        [EnumDataType(typeof(AppointmentServiceEnum))]
        public AppointmentServiceEnum? AppointmentService { get; set; }
        public string AppointmentNotes { get; set; }
    }
}
