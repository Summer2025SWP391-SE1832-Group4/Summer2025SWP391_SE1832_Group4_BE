using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class StandardARVRegimen
    {
        public int RegimenId { get; set; }

        [Required]
        [MaxLength(150)]
        public string RegimenName { get; set; } = string.Empty;

        public string? DetailedDescription { get; set; }

        [MaxLength(100)]
        public string? TargetPopulation { get; set; }

        [MaxLength(255)]
        public string? StandardDosage { get; set; }

        public string? Contraindications { get; set; }

        public string? CommonSideEffects { get; set; }

        // Navigation properties
        public virtual ICollection<PatientTreatment> PatientTreatments { get; set; } =
            new List<PatientTreatment>();
    }
}
