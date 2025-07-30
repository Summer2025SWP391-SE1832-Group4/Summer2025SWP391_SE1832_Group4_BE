using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class TestResultSummaryResponse
    {
        public int TotalTests { get; set; }
        public int PositiveCount { get; set; }
        public int NegativeCount { get; set; }
        public double PositivePercentage { get; set; }
        public double NegativePercentage { get; set; }
    }
}
