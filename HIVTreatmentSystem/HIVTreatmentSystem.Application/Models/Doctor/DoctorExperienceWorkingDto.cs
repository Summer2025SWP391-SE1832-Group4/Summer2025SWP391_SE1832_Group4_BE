namespace HIVTreatmentSystem.Application.Models.Doctor
{
    public class DoctorExperienceWorkingDto
    {
        public int ExperienceId { get; set; }
        public string HospitalName { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public int Years { get; set; }
    }
} 