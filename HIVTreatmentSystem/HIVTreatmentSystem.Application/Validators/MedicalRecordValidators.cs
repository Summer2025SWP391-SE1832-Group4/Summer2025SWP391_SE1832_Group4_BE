using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Models.Requests;

namespace HIVTreatmentSystem.Application.Validators
{
    /// <summary>
    /// Validator cho MedicalRecord requests
    /// Tách biệt validation logic khỏi business logic
    /// </summary>
    public static class MedicalRecordValidators
    {
        public static ValidationResult ValidateCreateRequest(MedicalRecordFromTestResultRequest request)
        {
            var result = new ValidationResult();

            result.AddErrorIf(request.TestResultId <= 0, "TestResultId is required and must be greater than 0")
                  .AddErrorIf(request.DoctorId <= 0, "DoctorId is required and must be greater than 0")
                  .AddErrorIf(request.ConsultationDate == default, "ConsultationDate is required")
                  .AddErrorIf(request.ConsultationDate > DateTime.Now, "ConsultationDate cannot be in the future");

            if (!string.IsNullOrWhiteSpace(request.UnderlyingDisease) && 
                request.UnderlyingDisease.Length > 255)
            {
                result.AddError("UnderlyingDisease cannot exceed 255 characters");
            }

            return result;
        }

        public static ValidationResult ValidateUpdateRequest(MedicalRecordFromTestResultRequest request)
        {
            var result = new ValidationResult();

            result.AddErrorIf(request.DoctorId <= 0, "DoctorId is required and must be greater than 0")
                  .AddErrorIf(request.ConsultationDate == default, "ConsultationDate is required")
                  .AddErrorIf(request.ConsultationDate > DateTime.Now, "ConsultationDate cannot be in the future");

            if (!string.IsNullOrWhiteSpace(request.UnderlyingDisease) && 
                request.UnderlyingDisease.Length > 255)
            {
                result.AddError("UnderlyingDisease cannot exceed 255 characters");
            }

            return result;
        }

        public static ValidationResult ValidatePatientIdRequest(int patientId)
        {
            var result = new ValidationResult();
            result.AddErrorIf(patientId <= 0, "PatientId must be greater than 0");
            return result;
        }
    }
} 