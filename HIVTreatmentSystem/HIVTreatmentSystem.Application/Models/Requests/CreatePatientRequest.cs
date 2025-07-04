using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class CreatePatientRequest
    {
        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        public string? Address { get; set; }

        public DateTime? HivDiagnosisDate { get; set; }

        public string? ConsentInformation { get; set; }

        public string? AnonymousIdentifier { get; set; }

        public string? AdditionalNotes { get; set; }

        public int AccountId { get; set; }

    }
}
