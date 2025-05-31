using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class TestResult
    {
        public int TestResultId { get; set; }

        public int PatientId { get; set; }

        public int? MedicalRecordId { get; set; }

        public DateTime TestDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string TestType { get; set; } = string.Empty;

        public int? CD4Count { get; set; }

        [MaxLength(20)]
        public string CD4Unit { get; set; } = "cells/mm³";

        [MaxLength(50)]
        public string? HivViralLoadValue { get; set; }

        [MaxLength(20)]
        public string HivViralLoadUnit { get; set; } = "copies/mL";

        [MaxLength(100)]
        public string? LabName { get; set; }

        [MaxLength(255)]
        public string? AttachedFileUrl { get; set; }

        public string? DoctorComments { get; set; }

        // Navigation properties
        public virtual Patient Patient { get; set; } = null!;
        public virtual MedicalRecord? MedicalRecord { get; set; }
    }
}
