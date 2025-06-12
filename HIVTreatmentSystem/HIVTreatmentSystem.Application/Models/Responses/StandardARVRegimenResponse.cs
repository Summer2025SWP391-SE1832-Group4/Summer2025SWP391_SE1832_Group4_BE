namespace HIVTreatmentSystem.Application.Models.Responses
{
    /// <summary>
    /// Response model for Standard ARV Regimen
    /// </summary>
    public class StandardARVRegimenResponse
    {
        /// <summary>
        /// Unique identifier for the regimen
        /// </summary>
        public int RegimenId { get; set; }

        /// <summary>
        /// Name of the ARV regimen
        /// </summary>
        public string RegimenName { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the regimen
        /// </summary>
        public string? DetailedDescription { get; set; }

        /// <summary>
        /// Target population for this regimen
        /// </summary>
        public string? TargetPopulation { get; set; }

        /// <summary>
        /// Standard dosage information
        /// </summary>
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