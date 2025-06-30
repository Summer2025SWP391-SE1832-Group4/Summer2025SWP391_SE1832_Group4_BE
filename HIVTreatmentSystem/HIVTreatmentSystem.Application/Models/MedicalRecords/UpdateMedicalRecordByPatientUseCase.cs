using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Application.Validators;

namespace HIVTreatmentSystem.Application.UseCases.MedicalRecords
{
    /// <summary>
    /// Use Case cho việc cập nhật Medical Record theo Patient ID
    /// Tuân thủ Single Responsibility Principle
    /// </summary>
    public class UpdateMedicalRecordByPatientUseCase
    {
        private readonly IMedicalRecordService _medicalRecordService;

        public UpdateMedicalRecordByPatientUseCase(IMedicalRecordService medicalRecordService)
        {
            _medicalRecordService = medicalRecordService;
        }

        /// <summary>
        /// Execute update operation với proper separation of concerns
        /// </summary>
        public async Task<Result<MedicalRecordResponse>> ExecuteAsync(
            int patientId, 
            MedicalRecordFromTestResultRequest request)
        {
            // 1. Validate input
            var patientValidation = MedicalRecordValidators.ValidatePatientIdRequest(patientId);
            if (!patientValidation.IsValid)
                return Result<MedicalRecordResponse>.ValidationFailure(patientValidation.Errors.ToArray());

            var requestValidation = MedicalRecordValidators.ValidateUpdateRequest(request);
            if (!requestValidation.IsValid)
                return Result<MedicalRecordResponse>.ValidationFailure(requestValidation.Errors.ToArray());

            try
            {
                // 2. Check if medical record exists
                var existingRecord = await _medicalRecordService.GetUniqueByPatientIdAsync(patientId);
                if (existingRecord == null)
                {
                    return Result<MedicalRecordResponse>.NotFound(
                        $"No medical record found for patient with ID {patientId}.");
                }

                // 3. Execute update operation
                var fullRequest = new MedicalRecordRequest
                {
                    PatientId = patientId,
                    DoctorId = request.DoctorId,
                    ConsultationDate = request.ConsultationDate,
                    Symptoms = request.Symptoms,
                    Diagnosis = request.Diagnosis,
                    DoctorNotes = request.DoctorNotes,
                    NextSteps = request.NextSteps,
                    UnderlyingDisease = request.UnderlyingDisease,
                    DrugAllergyHistory = request.DrugAllergyHistory
                };

                var updatedRecord = await _medicalRecordService.UpdateAsync(
                    existingRecord.MedicalRecordId, 
                    fullRequest);

                return Result<MedicalRecordResponse>.Success(updatedRecord);
            }
            catch (ArgumentException ex)
            {
                return Result<MedicalRecordResponse>.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                // Log exception here
                return Result<MedicalRecordResponse>.Failure(
                    "An unexpected error occurred while updating the medical record.");
            }
        }
    }
} 