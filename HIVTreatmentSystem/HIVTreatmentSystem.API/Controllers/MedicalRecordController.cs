using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Controller for managing Medical Records
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
            var medicalRecords = await _medicalRecordService.GetAllAsync();
            return Ok(new ApiResponse("Success", medicalRecords));
        }

        /// <summary>
        /// Get a medical record by ID
        /// </summary>
        /// <param name="id">The ID of the medical record to retrieve</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var medicalRecord = await _medicalRecordService.GetByIdAsync(id);
            if (medicalRecord == null)
                return NotFound(new ApiResponse("Medical record not found."));

            return Ok(new ApiResponse("Success", medicalRecord));
        }

        /// <summary>
        /// Get medical records by patient ID
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        [HttpGet("patient/{patientId}")]
        public async Task<IActionResult> GetByPatientId(int patientId)
        {
            var medicalRecords = await _medicalRecordService.GetByPatientIdAsync(patientId);
            return Ok(new ApiResponse("Success", medicalRecords));
        }

        /// <summary>
        /// Get medical records by doctor ID
        /// </summary>
        /// <param name="doctorId">The ID of the doctor</param>
        [HttpGet("doctor/{doctorId}")]
        public async Task<IActionResult> GetByDoctorId(int doctorId)
        {
            var medicalRecords = await _medicalRecordService.GetByDoctorIdAsync(doctorId);
            return Ok(new ApiResponse("Success", medicalRecords));
        }

        /// <summary>
        /// Create a new medical record
        /// </summary>
        /// <param name="request">The medical record data to create</param>
        [HttpPost]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create([FromBody] MedicalRecordCreateRequest request)
        {
            try
            {
                var mappedRequest = _mapper.Map<MedicalRecordRequest>(request);
                var medicalRecord = await _medicalRecordService.CreateAsync(mappedRequest);
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = medicalRecord.MedicalRecordId }, 
                    new ApiResponse("Medical record created successfully", medicalRecord)
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
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
        [Authorize(Roles = "Doctor")]
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
        /// Delete a medical record
        /// </summary>
        /// <param name="id">The ID of the medical record to delete</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
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
    }
} 