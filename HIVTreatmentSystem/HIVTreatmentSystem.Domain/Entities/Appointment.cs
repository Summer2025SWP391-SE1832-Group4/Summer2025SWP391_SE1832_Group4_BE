using System;
using HIVTreatmentSystem.Domain.Entities.Base;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Thực thể lịch hẹn khám bệnh.
    /// </summary>
    public class Appointment : BaseEntity<int>
    {
        public int PatientId { get; set; }
        public int? DoctorId { get; set; }
        /// <summary>
        /// Thời gian hẹn.
        /// </summary>
        public DateTime AppointmentTime { get; set; }  // ThoiGianHen
        /// <summary>
        /// Loại lịch hẹn.
        /// </summary>
        public string AppointmentType { get; set; }  // LoaiLichHen
        /// <summary>
        /// Trạng thái lịch hẹn.
        /// </summary>
        public string Status { get; set; }  // TrangThaiLichHen: DaLenLich, DaHoanThanh, DaHuyBenhNhan, DaHuyBacSi, VangMat, ChoXacNhan, DangDienRa
        /// <summary>
        /// Lý do khám.
        /// </summary>
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
