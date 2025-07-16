

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class CertificateRequest
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime IssuedDate { get; set; }
        public string IssuedBy { get; set; } = null!;
        public int DoctorId { get; set; }
    }

}
