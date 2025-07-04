

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class CertificateResponse
    {
        public int CertificateId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedBy { get; set; }
        public string DoctorName { get; set; }
    }
}
