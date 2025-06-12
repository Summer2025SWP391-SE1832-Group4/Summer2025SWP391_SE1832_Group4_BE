namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class FeedbackResponse
    {
        public int FeedbackId { get; set; }
        public int AppointmentId { get; set; }
        public int PatientId { get; set; }
        public int Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }
} 