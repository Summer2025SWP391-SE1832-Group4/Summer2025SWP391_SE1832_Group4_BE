

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class CreateDoctorRequest
    {

        public string? Qualifications { get; set; }

        public int? YearsOfExperience { get; set; }

        public string? ShortDescription { get; set; }

        public int AccountId { get; set; }
    }
}
