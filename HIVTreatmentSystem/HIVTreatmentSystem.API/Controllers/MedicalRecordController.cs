using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.UseCases.MedicalRecords;
using HIVTreatmentSystem.API.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Clean Architecture Controller for Medical Records
    /// Tuân thủ nguyên tắc Thin Controller, Fat Use Cases
    /// </summary>
    [Route("api/medical-records")]
    [ApiController]
    // [Authorize(Roles = "Admin,Doctor,Patient")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly CreateMedicalRecordFromTestResultUseCase _createFromTestResultUseCase;
        private readonly UpdateMedicalRecordByPatientUseCase _updateByPatientUseCase;
        private readonly IMapper _mapper;

        public MedicalRecordController(
            IMedicalRecordService medicalRecordService,
            CreateMedicalRecordFromTestResultUseCase createFromTestResultUseCase,
            UpdateMedicalRecordByPatientUseCase updateByPatientUseCase,
            IMapper mapper)
        {
            _medicalRecordService = medicalRecordService;
            _createFromTestResultUseCase = createFromTestResultUseCase;
            _updateByPatientUseCase = updateByPatientUseCase;
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
        /// Clean Architecture Implementation: Ultra-thin controller
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        /// <param name="request">The medical record data with test result ID</param>
        [HttpPost("patient/{patientId}/from-test-result")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateFromTestResultByPatientId(
            int patientId, 
            [FromBody] MedicalRecordFromTestResultRequest request)
        {
            var result = await _createFromTestResultUseCase.ExecuteAsync(patientId, request);
            
            return result.ToCreatedResult(this, 
                nameof(GetById), 
                new { id = result.Value?.MedicalRecordId }, 
                "Medical record created successfully from test result");
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
                mappedRequest.PatientId = patientId;

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
        /// Update medical record by patient ID
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
                mappedRequest.PatientId = patientId;

                var medicalRecord = await _medicalRecordService.CreateOrUpdateByPatientIdAsync(patientId, mappedRequest);
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
        /// Update medical record by patient ID with simplified request
        /// Clean Architecture Implementation: Ultra-thin controller
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        /// <param name="request">The updated medical record data</param>
        [HttpPut("patient/{patientId}/simple")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateSimpleByPatientId(
            int patientId, 
            [FromBody] MedicalRecordFromTestResultRequest request)
        {
            var result = await _updateByPatientUseCase.ExecuteAsync(patientId, request);
            return result.ToActionResult(this);
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
                return result 
                    ? Ok(new ApiResponse("Medical record deleted successfully"))
                    : NotFound(new ApiResponse("Medical record not found"));
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
        /// Delete medical record by patient ID
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        [HttpDelete("patient/{patientId}")]
        // [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteByPatientId(int patientId)
        {
            try
            {
                var existingRecord = await _medicalRecordService.GetUniqueByPatientIdAsync(patientId);
                if (existingRecord == null)
                    return NotFound(new ApiResponse($"No medical record found for patient with ID {patientId}."));

                var result = await _medicalRecordService.DeleteAsync(existingRecord.MedicalRecordId);
                return result 
                    ? Ok(new ApiResponse("Medical record deleted successfully"))
                    : BadRequest(new ApiResponse("Failed to delete medical record"));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while deleting the medical record."));
            }
        }
    }
} 