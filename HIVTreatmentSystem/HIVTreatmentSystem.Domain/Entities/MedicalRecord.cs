using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class MedicalRecord
    {
        public int MedicalRecordId { get; set; }

        public int AppointmentId { get; set; }

        public int PatientId { get; set; }

        public int DoctorId { get; set; }

        public DateTime ConsultationDate { get; set; }

        public string? Symptoms { get; set; }

        public string? Diagnosis { get; set; }

        public string? DoctorNotes { get; set; }

        public string? NextSteps { get; set; }

        [MaxLength(255)]
        public string? CoinfectionDiseases { get; set; }

        public string? DrugAllergyHistory { get; set; }

        // Navigation properties
        public virtual Appointment Appointment { get; set; } = null!;
        public virtual Patient Patient { get; set; } = null!;
        public virtual Doctor Doctor { get; set; } = null!;
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
    }
}
