using System;
using System.Collections.Generic;

namespace HIVTreatmentSystem.Application.Models.Doctor
{
    public class DoctorDetailDto
    {
        public int DoctorId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string? Specialty { get; set; }
        public string? Qualifications { get; set; }
        public int? YearsOfExperience { get; set; }
        public string? ShortDescription { get; set; }
        public string? ProfileImageUrl { get; set; }
        public List<ExperienceWorkingDto> ExperienceWorkings { get; set; } = new List<ExperienceWorkingDto>();
        public List<CertificateDto> Certificates { get; set; } = new List<CertificateDto>();
    }
} 