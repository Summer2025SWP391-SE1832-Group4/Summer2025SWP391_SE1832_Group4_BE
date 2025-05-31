using System;
using System.Collections.Generic;
using HIVTreatmentSystem.Domain.Entities.Base;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Patient : BaseEntity<int>
    {
        public string PatientCode { get; set; }  // MaBenhNhanTaiCoSo
        public DateTime? DateOfBirth { get; set; }
        public string Gender { get; set; }  // Nam, Nữ, Khác
        public string Address { get; set; }
        public DateTime? DiagnosisDate { get; set; }
        public string ConsentInformation { get; set; }  // ThongTinDongThuan
        public string AnonymousIdentifier { get; set; }  // MaSoDinhDanhAnDanh
        public string AdditionalNotes { get; set; }  // GhiChuThem
        public bool IsActive { get; set; } = true;

        // Foreign key
        public int AccountId { get; set; }  // Same as PatientId

        // Navigation properties
        public virtual Account Account { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; } = new List<Appointment>();
        public virtual ICollection<MedicalConsultation> MedicalConsultations { get; set; } = new List<MedicalConsultation>();
        public virtual ICollection<TestResult> TestResults { get; set; } = new List<TestResult>();
        public virtual ICollection<PatientTreatment> Treatments { get; set; } = new List<PatientTreatment>();
        public virtual ICollection<Reminder> Reminders { get; set; } = new List<Reminder>();
    }
}
