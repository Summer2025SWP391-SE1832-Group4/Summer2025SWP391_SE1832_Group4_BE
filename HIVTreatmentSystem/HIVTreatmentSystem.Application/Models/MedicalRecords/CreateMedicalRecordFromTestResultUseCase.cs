using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Validators;

namespace HIVTreatmentSystem.Application.UseCases.MedicalRecords
{
    /// <summary>
    /// Use Case cho việc tạo Medical Record từ Test Result
    /// Encapsulate business logic theo Clean Architecture
    /// </summary>
    public class CreateMedicalRecordFromTestResultUseCase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public CreateMedicalRecordFromTestResultUseCase(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        /// <summary>
        /// Execute use case với proper validation và error handling
        /// </summary>
        public async Task<Result<MedicalRecordResponse>> ExecuteAsync(
            int patientId, 
            MedicalRecordFromTestResultRequest request)
        {
            // 1. Validate input
            var patientValidation = MedicalRecordValidators.ValidatePatientIdRequest(patientId);
            if (!patientValidation.IsValid)
                return Result<MedicalRecordResponse>.ValidationFailure(patientValidation.Errors.ToArray());

            var requestValidation = MedicalRecordValidators.ValidateCreateRequest(request);
            if (!requestValidation.IsValid)
                return Result<MedicalRecordResponse>.ValidationFailure(requestValidation.Errors.ToArray());

            try
            {
                // 2. Execute business logic
                var medicalRecord = await _medicalRecordService.CreateFromTestResultAsync(request);
                
                // 3. Validate business rule: patient ID consistency
                if (medicalRecord.PatientId != patientId)
                {
                    return Result<MedicalRecordResponse>.Failure(
                        $"The test result does not belong to patient with ID {patientId}.");
                }

                return Result<MedicalRecordResponse>.Success(medicalRecord);
            }
            catch (ArgumentException ex)
            {
                return Result<MedicalRecordResponse>.Failure(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Result<MedicalRecordResponse>.Failure(ex.Message);
            }
            catch (Exception ex)
            {
                // Log exception here
                return Result<MedicalRecordResponse>.Failure(
                    "An unexpected error occurred while creating the medical record.");
            }
        }
    }
} 