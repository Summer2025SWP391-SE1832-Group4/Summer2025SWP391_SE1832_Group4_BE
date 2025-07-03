using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Enums;


namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IAdverseEffectReportService
    {
        Task<PageResult<AdverseEffectReportResponse>> GetAdverseEffectReportsAsync(
            int? accountId,
            DateOnly? dateOccurred,
            AdverseEffectSeverityEnum? severity,
            AdverseEffectReportStatusEnum? status,
            DateOnly? startDate,
            DateOnly? endDate,
            bool isDescending,
            string? sortBy,
            int pageIndex,
            int pageSize
        );

        Task<AdverseEffectReportResponse> GetByIdAsync(int id);

        Task<ApiResponse> CreateAsync(AdverseEffectReportRequest request);

        Task<ApiResponse> DeleteAsync(int id);

        Task<ApiResponse> UpdateAsync(int id, AdverseEffectReportUpdateRequest request);
    }
}
