
using System.ComponentModel.DataAnnotations;


namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class RegimenSuggestionRequest
    {
        [Required]
        public int cD4Count { get; set; }

        [Required]
        public string hivViralLoadValue { get; set; }
    }
}
