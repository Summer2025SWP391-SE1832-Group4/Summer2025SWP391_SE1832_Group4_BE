using System.Collections.Generic;
using HIVTreatmentSystem.Domain.Entities.Base;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Doctor : BaseEntity<int>
    {
        public string Specialization { get; set; }  // ChuyenMon
        public string Qualifications { get; set; }  // BangCap
        public int YearsOfExperience { get; set; }  // SoNamKinhNghiem
        public string ShortDescription { get; set; }  // MoTaNgan
        
        // Foreign key
        public int AccountId { get; set; }  // Same as DoctorId
        
        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<MedicalConsultation> MedicalConsultations { get; set; } = new List<MedicalConsultation>();
        public virtual ICollection<PatientTreatment> PrescribedTreatments { get; set; } = new List<PatientTreatment>();
        public virtual ICollection<DoctorSchedule> Schedules { get; set; } = new List<DoctorSchedule>();
    }
}
