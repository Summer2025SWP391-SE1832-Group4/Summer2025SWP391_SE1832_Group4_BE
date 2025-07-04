

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class RegimenSuggestionResponse
    {
        public int RegimenId { get; set; }
        public string RegimenName { get; set; } = string.Empty;
        public string SuggestionLevel { get; set; } = string.Empty;
    }
}
