using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IAdverseEffectReportRepository
    {
        Task<IEnumerable<AdverseEffectReport>> GetAllAsync(
            int? accountId,
            DateOnly? dateOccurred,
            AdverseEffectSeverityEnum? severity,
            AdverseEffectReportStatusEnum? status,
            DateOnly? startDate,
            DateOnly? endDate,
            bool isDescending,
            string? sortBy
        );

        Task<AdverseEffectReport?> GetByIdAsync(int id);
        Task<bool> AddAsync(AdverseEffectReport report);

        Task DeleteAsync(AdverseEffectReport report);

        Task<bool> UpdateAsync(AdverseEffectReport report);
    }
}
