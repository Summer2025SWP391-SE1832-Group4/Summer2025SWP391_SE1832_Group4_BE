using System;
using HIVTreatmentSystem.Domain.Entities.Base;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Appointment : BaseEntity<int>
    {
        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        public DateTime AppointmentTime { get; set; }  // ThoiGianHen
        public string AppointmentType { get; set; }  // LoaiLichHen
        public string Status { get; set; }  // TrangThaiLichHen: DaLenLich, DaHoanThanh, DaHuyBenhNhan, DaHuyBacSi, VangMat, ChoXacNhan, DangDienRa
        public string Reason { get; set; }  // LyDoKham
        public string Notes { get; set; }  // GhiChuHen
        public bool IsAnonymousConsultation { get; set; } = false;  // LaTuVanAnDanh
        public int? CreatedByAccountId { get; set; }  // NguoiTaoLichHenID
        
        // Navigation properties
        public virtual Patient Patient { get; set; }
        public virtual Doctor Doctor { get; set; }
        public virtual Account CreatedByAccount { get; set; }
        public virtual MedicalConsultation MedicalConsultation { get; set; }
        public virtual Reminder Reminder { get; set; }
    }
}
