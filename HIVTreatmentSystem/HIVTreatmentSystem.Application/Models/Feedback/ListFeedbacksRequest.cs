namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class ListFeedbacksRequest
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int? PatientId { get; set; }
        public int? AppointmentId { get; set; }
        public int? DoctorId { get; set; }
        public int? Rating { get; set; }
    }
} 