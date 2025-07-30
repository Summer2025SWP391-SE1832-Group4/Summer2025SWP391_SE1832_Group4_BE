using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class DashboardStatistics
    {
        public string Period { get; set; } = string.Empty;
        public DateTime? Date { get; set; }
        public int Count { get; set; }
        public double? Percentage { get; set; }
    }
}
