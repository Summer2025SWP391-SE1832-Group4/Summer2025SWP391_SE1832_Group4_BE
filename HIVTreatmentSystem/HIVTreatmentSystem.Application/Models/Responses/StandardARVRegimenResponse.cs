namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class StandardARVRegimenResponse
    {
        public int RegimenId { get; set; }
        public string RegimenName { get; set; } = null!;
        public string? DetailedDescription { get; set; }
        public string? TargetPopulation { get; set; }
        public string? StandardDosage { get; set; }
        public string? Contraindications { get; set; }
        public string? CommonSideEffects { get; set; }
    }
}
