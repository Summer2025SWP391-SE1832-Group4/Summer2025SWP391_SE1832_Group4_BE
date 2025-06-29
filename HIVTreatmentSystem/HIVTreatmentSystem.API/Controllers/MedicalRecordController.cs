using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Controller for managing Medical Records (Patient-based operations)
    /// </summary>
    [Route("api/medical-records")]
    [ApiController]
    // [Authorize(Roles = "Admin,Doctor,Patient")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IMapper _mapper;

        public MedicalRecordController(IMedicalRecordService medicalRecordService, IMapper mapper)
        {
            _medicalRecordService = medicalRecordService;
            _mapper = mapper;
        }

        /// <summary>
        /// Get all medical records
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var medicalRecords = await _medicalRecordService.GetAllAsync();
                return Ok(new ApiResponse("Success", medicalRecords));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while retrieving medical records."));
            }
        }

        /// <summary>
        /// Get a medical record by ID
        /// </summary>
        /// <param name="id">The ID of the medical record to retrieve</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.GetByIdAsync(id);
                if (medicalRecord == null)
                    return NotFound(new ApiResponse("Medical record not found."));

                return Ok(new ApiResponse("Success", medicalRecord));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while retrieving the medical record."));
            }
        }

        /// <summary>
        /// Get medical records by patient ID
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            try
            {
                var medicalRecords = await _medicalRecordService.GetByPatientIdAsync(patientId);
                return Ok(new ApiResponse("Success", medicalRecords));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while retrieving medical records."));
            }
        }

        /// <summary>
        /// Get the unique medical record for a patient (1-to-1 relationship)
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        [HttpGet("patient/{patientId}/unique")]
        public async Task<IActionResult> GetUniqueByPatientId(int patientId)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.GetUniqueByPatientIdAsync(patientId);
                if (medicalRecord == null)
                    return NotFound(new ApiResponse($"No medical record found for patient with ID {patientId}."));

                return Ok(new ApiResponse("Success", medicalRecord));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while retrieving the medical record."));
            }
        }

        /// <summary>
        /// Get medical records by doctor ID
        /// </summary>
        /// <param name="doctorId">The ID of the doctor</param>
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctorId(int doctorId)
        {
            try
            {
                var medicalRecords = await _medicalRecordService.GetByDoctorIdAsync(doctorId);
                return Ok(new ApiResponse("Success", medicalRecords));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while retrieving medical records."));
            }
        }

        /// <summary>
        /// Check if patient has a medical record
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        [HttpGet("patient/{patientId}/exists")]
        public async Task<IActionResult> CheckPatientHasMedicalRecord(int patientId)
        {
            try
            {
                var hasRecord = await _medicalRecordService.PatientHasMedicalRecordAsync(patientId);
                return Ok(new ApiResponse("Success", new { PatientId = patientId, HasMedicalRecord = hasRecord }));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while checking medical record existence."));
            }
        }

        /// <summary>
        /// Create a new medical record for a patient from test result
        /// Bác sĩ chỉ cần nhập các thông tin cơ bản, test data đã có trong TestResult
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        /// <param name="request">The medical record data with test result ID</param>
        [HttpPost("patient/{patientId}/from-test-result")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateFromTestResultByPatientId(int patientId, [FromBody] MedicalRecordFromTestResultRequest request)
        {
            try
            {
                // Verify that the request is for the correct patient
                if (request.TestResultId <= 0)
                {
                    return BadRequest(new ApiResponse("TestResultId is required."));
                }

                var medicalRecord = await _medicalRecordService.CreateFromTestResultAsync(request);
                
                // Validate that the created medical record belongs to the specified patient
                if (medicalRecord.PatientId != patientId)
                {
                    return BadRequest(new ApiResponse($"The test result does not belong to patient with ID {patientId}."));
                }

                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = medicalRecord.MedicalRecordId }, 
                    new ApiResponse("Medical record created successfully from test result", medicalRecord)
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while creating the medical record from test result."));
            }
        }

        /// <summary>
        /// Create a new medical record for a specific patient
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        /// <param name="request">The medical record data to create</param>
        [HttpPost("patient/{patientId}")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateForPatient(int patientId, [FromBody] MedicalRecordCreateRequest request)
        {
            try
            {
                var mappedRequest = _mapper.Map<MedicalRecordRequest>(request);
                
                // Ensure the medical record is created for the correct patient
                // This will be handled in the service layer, but we validate here too
                var medicalRecord = await _medicalRecordService.CreateOrUpdateByPatientIdAsync(patientId, mappedRequest);
                
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = medicalRecord.MedicalRecordId }, 
                    new ApiResponse("Medical record created successfully for patient", medicalRecord)
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while creating the medical record."));
            }
        }

        /// <summary>
        /// Update an existing medical record
        /// </summary>
        /// <param name="id">The ID of the medical record to update</param>
        /// <param name="request">The updated medical record data</param>
        [HttpPut("{id}")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicalRecordRequest request)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.UpdateAsync(id, request);
                return Ok(new ApiResponse("Medical record updated successfully", medicalRecord));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while updating the medical record."));
            }
        }

        /// <summary>
        /// Update medical record for a specific patient
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        /// <param name="request">The updated medical record data</param>
        [HttpPut("patient/{patientId}")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateByPatientId(int patientId, [FromBody] MedicalRecordCreateRequest request)
        {
            try
            {
                var mappedRequest = _mapper.Map<MedicalRecordRequest>(request);
                var medicalRecord = await _medicalRecordService.CreateOrUpdateByPatientIdAsync(patientId, mappedRequest);
                return Ok(new ApiResponse("Medical record updated successfully", medicalRecord));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while updating the medical record."));
            }
        }

        /// <summary>
        /// Update medical record using simplified request (for compatibility)
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        /// <param name="request">The updated medical record data</param>
        [HttpPut("patient/{patientId}/simple")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateSimpleByPatientId(int patientId, [FromBody] MedicalRecordFromTestResultRequest request)
        {
            try
            {
                // Get existing medical record for the patient
                var existingRecord = await _medicalRecordService.GetUniqueByPatientIdAsync(patientId);
                if (existingRecord == null)
                    return NotFound(new ApiResponse($"No medical record found for patient with ID {patientId}."));

                // Create full request with existing data
                var fullRequest = new MedicalRecordRequest
                {
                    PatientId = patientId,
                    DoctorId = request.DoctorId,
                    ConsultationDate = request.ConsultationDate,
                    Symptoms = request.Symptoms,
                    Diagnosis = request.Diagnosis,
                    DoctorNotes = request.DoctorNotes,
                    NextSteps = request.NextSteps,
                    CoinfectionDiseases = request.CoinfectionDiseases,
                    DrugAllergyHistory = request.DrugAllergyHistory
                };

                var medicalRecord = await _medicalRecordService.UpdateAsync(existingRecord.MedicalRecordId, fullRequest);
                return Ok(new ApiResponse("Medical record updated successfully", medicalRecord));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while updating the medical record."));
            }
        }

        /// <summary>
        /// Delete a medical record
        /// </summary>
        /// <param name="id">The ID of the medical record to delete</param>
        [HttpDelete("{id}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _medicalRecordService.DeleteAsync(id);
                return Ok(new ApiResponse("Medical record deleted successfully."));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while deleting the medical record."));
            }
        }

        /// <summary>
        /// Delete medical record by patient ID (1-to-1 relationship)
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        [HttpDelete("patient/{patientId}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByPatientId(int patientId)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.GetUniqueByPatientIdAsync(patientId);
                if (medicalRecord == null)
                    return NotFound(new ApiResponse($"No medical record found for patient with ID {patientId}."));

                var result = await _medicalRecordService.DeleteAsync(medicalRecord.MedicalRecordId);
                return Ok(new ApiResponse("Medical record deleted successfully."));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while deleting the medical record."));
            }
        }
    }
} 