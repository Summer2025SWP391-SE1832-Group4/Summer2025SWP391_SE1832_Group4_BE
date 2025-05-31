using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Common;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class ARVStandard : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [StringLength(50)]
        public string Version { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime EffectiveDate { get; set; }

        [StringLength(1000)]
        public string Guidelines { get; set; }

        // Navigation Properties
        public virtual ICollection<PatientRecord> PatientRecords { get; set; } =
            new List<PatientRecord>();
    }
}
