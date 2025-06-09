using System;

namespace HIVTreatmentSystem.Application.Models.Doctor
{
    public class ExperienceWorkingDto
    {
        public int ExperienceId { get; set; }
        public int DoctorId { get; set; }
        public string HospitalName { get; set; }
        public string Position { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
    }
} 