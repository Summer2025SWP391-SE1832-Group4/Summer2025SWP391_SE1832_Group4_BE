using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Common;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class SystemMetric : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string MetricName { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MetricValue { get; set; }

        [StringLength(50)]
        public string MetricUnit { get; set; }

        public DateTime RecordDate { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        [StringLength(500)]
        public string Description { get; set; }
    }
}
