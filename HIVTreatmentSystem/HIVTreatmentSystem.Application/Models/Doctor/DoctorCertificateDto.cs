namespace HIVTreatmentSystem.Application.Models.Doctor
{
    public class DoctorCertificateDto
    {
        public int CertificateId { get; set; }
        public string CertificateName { get; set; } = string.Empty;
        public string IssuedBy { get; set; } = string.Empty;
        public int Year { get; set; }
    }
} 