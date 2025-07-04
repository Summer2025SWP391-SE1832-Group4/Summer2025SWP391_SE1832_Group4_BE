
namespace HIVTreatmentSystem.Application.Models.Doctor
{
    public class CertificateDto
    {
        public int CertificateId { get; set; }
        public int DoctorId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime IssuedDate { get; set; }
        public string IssuedBy { get; set; }
    }
} 