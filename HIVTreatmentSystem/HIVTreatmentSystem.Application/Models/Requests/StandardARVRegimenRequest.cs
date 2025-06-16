using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class StandardARVRegimenRequest
    {
        [Required, MaxLength(150)]
        public string RegimenName { get; set; } = null!;

        public string? DetailedDescription { get; set; }

        [MaxLength(100)]
        public string? TargetPopulation { get; set; }

        [MaxLength(255)]
        public string? StandardDosage { get; set; }

        public string? Contraindications { get; set; }

        public string? CommonSideEffects { get; set; }
    }
}
