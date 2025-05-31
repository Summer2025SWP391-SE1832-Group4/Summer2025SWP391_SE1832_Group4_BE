using System.Collections.Generic;
using HIVTreatmentSystem.Domain.Entities.Base;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Thực thể bác sĩ.
    /// </summary>
    public class Doctor : BaseEntity<int>
    {
        /// <summary>
        /// Chuyên môn.
        /// </summary>
        public string Specialization { get; set; }  // ChuyenMon
        /// <summary>
        /// Bằng cấp.
        /// </summary>
        public string Qualifications { get; set; }  // BangCap
        /// <summary>
        /// Số năm kinh nghiệm.
        /// </summary>
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
