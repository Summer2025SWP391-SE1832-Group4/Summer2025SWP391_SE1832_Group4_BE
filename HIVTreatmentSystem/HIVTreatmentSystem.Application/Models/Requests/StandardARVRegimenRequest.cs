using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    /// <summary>
    /// Request model for creating or updating a Standard ARV Regimen
    /// </summary>
    public class StandardARVRegimenRequest
    {
        /// <summary>
        /// Name of the ARV regimen
        /// </summary>
        [Required(ErrorMessage = "Regimen name is required")]
        [MaxLength(150, ErrorMessage = "Regimen name cannot exceed 150 characters")]
        public string RegimenName { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the regimen
        /// </summary>
        public string? DetailedDescription { get; set; }

        /// <summary>
        /// Target population for this regimen
        /// </summary>
        [MaxLength(100, ErrorMessage = "Target population cannot exceed 100 characters")]
        public string? TargetPopulation { get; set; }

        /// <summary>
        /// Standard dosage information
        /// </summary>
        [MaxLength(255, ErrorMessage = "Standard dosage cannot exceed 255 characters")]
        public string? StandardDosage { get; set; }

        /// <summary>
        /// Contraindications for this regimen
        /// </summary>
        public string? Contraindications { get; set; }

        /// <summary>
        /// Common side effects of this regimen
        /// </summary>
        public string? CommonSideEffects { get; set; }
    }
} 