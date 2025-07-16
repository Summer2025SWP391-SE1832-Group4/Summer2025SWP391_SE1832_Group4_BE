

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class DoctorResponse
    {
        public int DoctorId { get; set; } 

        public string? Specialty { get; set; }

        public string? Qualifications { get; set; }

        public int? YearsOfExperience { get; set; }

        public string? ShortDescription { get; set; }

        public AccountResponse? Account { get; set; }
    }
}
