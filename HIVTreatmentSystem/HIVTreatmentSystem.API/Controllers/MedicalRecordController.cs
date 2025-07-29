using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Clean Architecture Controller for Medical Records
    /// </summary>
    [Route("api/medical-records")]
    [ApiController]
    // [Authorize(Roles = "Admin,Doctor,Patient")]
    public class MedicalRecordController : ControllerBase
    {
        private readonly IMedicalRecordService _medicalRecordService;
        private readonly IPatientTreatmentService _patientTreatmentService;
        private readonly IAccountRepository _accountRepository;
        private readonly IPatientService _patientService;
        private readonly IMapper _mapper;

        public MedicalRecordController(
            IMedicalRecordService medicalRecordService,
            IPatientTreatmentService patientTreatmentService,
            IAccountRepository accountRepository,
            IPatientService patientService,
            IMapper mapper)
        {
            _medicalRecordService = medicalRecordService;
            _patientTreatmentService = patientTreatmentService;
            _accountRepository = accountRepository;
            _patientService = patientService;
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
                var patientTreatments = await _patientTreatmentService.GetByPatientIdAsync(patientId);
                // Populate treatments into each medical record response
                foreach (var record in medicalRecords)
                {
                    record.PatientTreatments = patientTreatments.ToList();
                }
                // Return only the updated medical records
                return Ok(new ApiResponse("Success", medicalRecords));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while retrieving medical records."));
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="phone">The phone of the patient</param>
        [HttpGet("patient/{phone}/unique")]
        public async Task<IActionResult> GetUniqueByPatientPhone(string phone)
        {
            try
            {
                var medicalRecords = await _medicalRecordService.GetUniqueByPatientPhoneAsync(phone);
                if (medicalRecords == null || !medicalRecords.Any())
                    return NotFound(new ApiResponse($"No medical records found for patient with phone {phone}."));

                return Ok(new ApiResponse("Success", medicalRecords));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while retrieving medical records."));
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
        /// Create a new medical record
        /// </summary>
        /// <param name="request">The medical record data to create</param>
        [HttpPost]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> Create([FromQuery] MedicalRecordRequest request)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.CreateAsync(request);
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
        /// Create a new medical record based on patient ID
        /// </summary>
        /// <param name="request">The medical record data with patient ID</param>
        [HttpPost("by-patient")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateByPatient([FromBody] MedicalRecordByPatientRequest request)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.CreateByPatientIdAsync(request);
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
        /// Create a new medical record from test result
        /// </summary>
        /// <param name="request">The medical record data with test result ID</param>
        [HttpPost("from-test-result")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateFromTestResult([FromBody] MedicalRecordFromTestResultRequest request)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.CreateFromTestResultAsync(request);
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
                return StatusCode(500, new ApiResponse("An error occurred while creating the medical record."));
            }
        }

        /// <summary>
        /// Create a new medical record for a patient from test result
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        /// <param name="request">The medical record data with test result ID</param>
        [HttpPost("patient/{patientId}/from-test-result")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> CreateFromTestResultByPatientId(
            int patientId, 
            [FromBody] MedicalRecordFromTestResultRequest request)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.CreateFromTestResultAsync(request);
                
                // Validate business rule: patient ID consistency
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
                return StatusCode(500, new ApiResponse("An error occurred while creating the medical record."));
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
        /// </summary>
        /// <param name="patientId">The ID of the patient</param>
        /// <param name="request">The updated medical record data</param>
        [HttpPut("patient/{patientId}/simple")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> UpdateSimpleByPatientId(
            int patientId, 
            [FromBody] MedicalRecordFromTestResultRequest request)
        {
            try
            {
                // Check if medical record exists
                var records = await _medicalRecordService.GetByPatientIdAsync(patientId);
                var existingRecord = records.FirstOrDefault();
                if (existingRecord == null)
                {
                    return NotFound(new ApiResponse($"No medical record found for patient with ID {patientId}."));
                }

                // Convert to full request and update
                var fullRequest = new MedicalRecordRequest
                {
                    PatientId = patientId,
                    DoctorId = request.DoctorId,
                    ConsultationDate = request.ConsultationDate,
                    Symptoms = request.Symptoms,
                    Diagnosis = request.Diagnosis,
                    PregnancyStatus = request.PregnancyStatus,
                    PregnancyWeek = request.PregnancyWeek,
                    DoctorNotes = request.DoctorNotes,
                    NextSteps = request.NextSteps,
                    UnderlyingDisease = request.UnderlyingDisease,
                    DrugAllergyHistory = request.DrugAllergyHistory
                };

                var updatedRecord = await _medicalRecordService.UpdateAsync(existingRecord.MedicalRecordId, fullRequest);
                return Ok(new ApiResponse("Medical record updated successfully", updatedRecord));
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
                var records = await _medicalRecordService.GetByPatientIdAsync(patientId);
                var existingRecord = records.FirstOrDefault();
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

        /// <summary>
        /// Add test result to existing medical record
        /// Thêm test result vào medical record đã tồn tại
        /// </summary>
        /// <param name="medicalRecordId">The ID of the medical record</param>
        /// <param name="request">Request chứa TestResultId</param>
        [HttpPost("{medicalRecordId}/add-test-result")]
        // [Authorize(Roles = "Doctor")]
        public async Task<IActionResult> AddTestResultToMedicalRecord(
            int medicalRecordId, 
            [FromBody] AddTestResultToMedicalRecordRequest request)
        {
            try
            {
                var medicalRecord = await _medicalRecordService.AddTestResultToMedicalRecordAsync(medicalRecordId, request);
                return Ok(new ApiResponse("Test result added to medical record successfully", medicalRecord));
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
                return StatusCode(500, new ApiResponse("An error occurred while adding test result to medical record."));
            }
        }
    }
} 