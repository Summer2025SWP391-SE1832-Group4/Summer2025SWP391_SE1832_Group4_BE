using System;

namespace HIVTreatmentSystem.Application.Models.ExperienceWorking
{
    public class ExperienceWorkingDto
    {
        public int DoctorId { get; set; }
        public string HospitalName { get; set; }
        public string Position { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
} 