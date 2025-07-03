using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class RegimenSuggestionResponse
    {
        public int RegimenId { get; set; }
        public string RegimenName { get; set; } = string.Empty;
        public string SuggestionLevel { get; set; } = string.Empty;
    }
}
