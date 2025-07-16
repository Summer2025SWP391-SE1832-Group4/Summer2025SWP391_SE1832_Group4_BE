using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class AppointmentResponse
    {
        public int AppointmentId { get; set; }
        public DateOnly AppointmentDate { get; set; }
        public TimeOnly AppointmentTime { get; set; }
        public AppointmentTypeEnum AppointmentType { get; set; }
        public AppointmentStatus Status { get; set; }
        public AppointmentServiceEnum AppointmentService { get; set; }

        public string? PatientName { get; set; }

        public virtual PatientResponse Patient { get; set; } = null!;
        public string? DoctorName { get; set; }

        public virtual DoctorResponse? Doctor { get; set; }
    }
}
