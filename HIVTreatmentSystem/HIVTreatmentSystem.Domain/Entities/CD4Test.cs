using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Common;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class CD4Test : BaseEntity
    {
        public int PatientId { get; set; }

        public DateTime TestDate { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal CD4Count { get; set; }

        [Column(TypeName = "decimal(15,2)")]
        public decimal? ViralLoad { get; set; }

        public TestResult Result { get; set; }

        [StringLength(500)]
        public string Notes { get; set; }

        [StringLength(100)]
        public string LabTechnician { get; set; }

        [StringLength(100)]
        public string LabName { get; set; }

        public string TestReportUrl { get; set; }

        // Navigation Properties
        [ForeignKey("PatientId")]
        public virtual PatientRecord Patient { get; set; }
    }
}
