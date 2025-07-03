using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class PatientTreatment
    {
        public int PatientTreatmentId { get; set; }

        public int PatientId { get; set; }

        public int RegimenId { get; set; }

        public int PrescribingDoctorId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? ExpectedEndDate { get; set; }

        public string? RegimenAdjustments { get; set; }
        public int? BaselineCD4 { get; set; }

        [MaxLength(50)]
        public string? BaselineHivViralLoad { get; set; }

        [MaxLength(255)]
        public string? ActualDosage { get; set; }

        public TreatmentStatus Status { get; set; } = TreatmentStatus.InTreatment;

        public string? ReasonForChangeOrStop { get; set; }

        // Navigation properties
        public virtual Patient Patient { get; set; } = null!;
        public virtual StandardARVRegimen Regimen { get; set; } = null!;
        public virtual Doctor PrescribingDoctor { get; set; } = null!;
        public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
