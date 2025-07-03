using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
