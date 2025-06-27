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

        /// <summary>
        /// ID của appointment liên quan đến test result này (nullable vì có thể có test không liên quan đến appointment cụ thể)
        /// </summary>
        public int? AppointmentId { get; set; }

        public DateTime TestDate { get; set; }

        [Required]
        [MaxLength(100)]
        public string TestType { get; set; } = string.Empty;

        public int? CD4Count { get; set; }

        // [MaxLength(20)]
        // public string CD4Unit { get; set; } = "cells/mm³";

        [MaxLength(50)]
        public string? HivViralLoadValue { get; set; }

        // [MaxLength(20)]
        // public string HivViralLoadUnit { get; set; } = "copies/mL";

        [MaxLength(100)]
        public string? LabName { get; set; }

        public string? DoctorComments { get; set; }//Final result
        public string? TestResults { get; set; }

        // Navigation properties
        public virtual Patient Patient { get; set; } = null!;
        public virtual MedicalRecord? MedicalRecord { get; set; }
        public virtual Appointment? Appointment { get; set; }
    }
}
