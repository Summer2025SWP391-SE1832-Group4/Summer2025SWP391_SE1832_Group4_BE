using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class DashboardStatisticsResponse
    {
        public string Period { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public int Count { get; set; }
        public double? Percentage { get; set; }
    }
}
