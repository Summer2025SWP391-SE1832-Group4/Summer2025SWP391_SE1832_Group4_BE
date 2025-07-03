using AutoMapper;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Pages;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Repositories;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Interfaces;


namespace HIVTreatmentSystem.Application.Services.AdverseEffectReport
{
    public class AdverseEffectReportService : IAdverseEffectReportService
    {
        private readonly IAdverseEffectReportRepository _adverseEffectReportRepository;
        private readonly IMapper _mapper;
        private readonly IPatientRepository _patientRepository;
        private readonly ITestResultRepository _testResultRepository;

        public AdverseEffectReportService(IAdverseEffectReportRepository adverseEffectReportRepository, IMapper mapper, IPatientRepository patientRepository, ITestResultRepository testResultRepository)
        {
            _adverseEffectReportRepository = adverseEffectReportRepository;
            _mapper = mapper;
            _patientRepository = patientRepository;
            _testResultRepository = testResultRepository;
        }

        public async Task<PageResult<AdverseEffectReportResponse>> GetAdverseEffectReportsAsync(
            int? accountId,
            DateOnly? dateOccurred,
            AdverseEffectSeverityEnum? severity,
            AdverseEffectReportStatusEnum? status,
            DateOnly? startDate,
            DateOnly? endDate,
            bool isDescending,
            string? sortBy,
            int pageIndex,
            int pageSize)
        {
            var reports = await _adverseEffectReportRepository.GetAllAsync(
                accountId,
                dateOccurred,
                severity,
                status,
                startDate,
                endDate,
                isDescending,
                sortBy
            );

            var paged = reports.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            var dtoList = _mapper.Map<List<AdverseEffectReportResponse>>(paged);

            return new PageResult<AdverseEffectReportResponse>(
                dtoList,
                pageSize,
                pageIndex,
                reports.Count()
            );
        }

        public async Task<AdverseEffectReportResponse?> GetByIdAsync(int id)
        {
            var report = await _adverseEffectReportRepository.GetByIdAsync(id);

            if (report == null)
                return null;

            return _mapper.Map<AdverseEffectReportResponse>(report);
        }

        public async Task<ApiResponse> CreateAsync(AdverseEffectReportRequest request)
        {
            try
            {
                var patient = await _patientRepository.GetByIdAsync(request.PatientId);
                if (patient == null)
                {
                    return new ApiResponse
                    (
                        $"Error: Patient with ID {request.PatientId} does not exist."
                    );
                }

                //var testResult = await _testResultRepository.GetByPatientIdAsync(request.PatientId);
                //if (testResult == null || !testResult.Any())
                //{
                //    return new ApiResponse
                //    (
                //        $"Error: You have not tested for HIV and started treatment."
                //    );
                //}

                var report = _mapper.Map<HIVTreatmentSystem.Domain.Entities.AdverseEffectReport>(request);
                var result = await _adverseEffectReportRepository.AddAsync(report);
                if (result)
                {
                    return new ApiResponse("AdverseEffectReport created successfully.");
                } else {                     
                    return new ApiResponse("Failed to create AdverseEffectReport."); };
            }
            catch (Exception ex)
            {
                return new ApiResponse
                (
                    $"An error occurred while creating the report: {ex.InnerException.Message}"
                );
            }
        }

        public async Task<ApiResponse> DeleteAsync(int id)
        {
            var report = await _adverseEffectReportRepository.GetByIdAsync(id);
            if (report == null)
            {
                return new ApiResponse("Error: Adverse Effect Report not found");
            }

            await _adverseEffectReportRepository.DeleteAsync(report);
            return new ApiResponse("Adverse Effect Report deleted successfully.");
        }

        public async Task<ApiResponse> UpdateAsync(int id, AdverseEffectReportUpdateRequest request)
        {
            var report = await _adverseEffectReportRepository.GetByIdAsync(id);
            if (report == null)
            {
                return new ApiResponse("Error: Adverse Effect Report not found");
            }
            if (!string.IsNullOrWhiteSpace(request.Description))
            {
                report.Description = request.Description;
            }
            if (request.DateOccurred.HasValue)
            {
                report.DateOccurred = request.DateOccurred.Value;
            }
            if (request.Status.HasValue)
            {
                report.Status = request.Status.Value;
            }
            if (request.Severity.HasValue)
            {
                report.Severity = request.Severity.Value;
            }

            var result = await _adverseEffectReportRepository.UpdateAsync(report);
            if (result)
            {
                return new ApiResponse("Adverse Effect Report updated successfully.");
            }
            else
            {
                return new ApiResponse("Failed to update Adverse Effect Report.");
            }
        }
    }
}
