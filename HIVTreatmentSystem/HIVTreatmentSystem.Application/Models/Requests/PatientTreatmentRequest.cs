using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class PatientTreatmentRequest
    {
        [Required]
        public int PatientId { get; set; }

        [Required]
        public int RegimenId { get; set; }

        [Required]
        public int PrescribingDoctorId { get; set; }

        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? ExpectedEndDate { get; set; }
        public string? RegimenAdjustments { get; set; }
        public int? BaselineCD4 { get; set; }
        public string? BaselineHivViralLoad { get; set; }
        public string? ActualDosage { get; set; }
        public TreatmentStatus Status { get; set; } = TreatmentStatus.InTreatment;
        public string? ReasonForChangeOrStop { get; set; }
    }
}
