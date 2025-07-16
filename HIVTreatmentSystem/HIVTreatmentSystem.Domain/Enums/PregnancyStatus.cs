

namespace HIVTreatmentSystem.Domain.Enums
{
    public enum PregnancyStatus
    {
        /// <summary>
        /// Không mang thai
        /// </summary>
        NotPregnant,
        
        /// <summary>
        /// Đang mang thai
        /// </summary>
        Pregnant,
        
        /// <summary>
        /// Chưa xác định được trạng thái mang thai
        /// </summary>
        Unknown,
        
        /// <summary>
        /// Không áp dụng (dành cho nam giới)
        /// </summary>
        NotApplicable
    }
} 