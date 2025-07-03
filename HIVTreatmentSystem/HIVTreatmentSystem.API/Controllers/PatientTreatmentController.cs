using HIVTreatmentSystem.API.Models;
using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// CRUD operations for Patient Treatments
    /// </summary>
    [ApiController]
    [Route("api/patient-treatment")]
    // [Authorize]
    public class PatientTreatmentController : ControllerBase
    {
        private readonly IPatientTreatmentService _service;
        private readonly IPatientService _patientService;
        private readonly IDoctorService _doctorService;

        public PatientTreatmentController(
            IPatientTreatmentService service,
            IPatientService patientService,
            IDoctorService doctorService)
        {
            _service = service;
            _patientService = patientService;
            _doctorService = doctorService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientTreatmentResponse>>>> GetAll()
        {
            var list = await _service.GetAllAsync();
            if(list == null || !list.Any())
            {
                return NotFound(new ApiResponse("No treatments found"));
            }
            return Ok(new ApiResponse("Success", list));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<PatientTreatmentResponse>>> GetById(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null) return NotFound(new ApiResponse("Treatment not found"));
            return Ok(new ApiResponse("Success", item));
        }

        [HttpGet("patient/{patientId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientTreatmentResponse>>>> GetByPatient(int patientId)
        {
            // Validate patient exists first
            try
            {
                var patient = await _patientService.GetPatientByIdAsync(patientId);
                if (patient == null)
                {
                    return NotFound(new ApiResponse($"Patient with ID {patientId} not found"));
                }
            }
            catch (Exception)
            {
                return NotFound(new ApiResponse($"Patient with ID {patientId} not found"));
            }

            var list = await _service.GetByPatientIdAsync(patientId);
            return Ok(new ApiResponse("Success", list));
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientTreatmentResponse>>>> GetByDoctor(int doctorId)
        {
            // Validate doctor exists first
            try
            {
                var doctor = await _doctorService.GetDoctorByIdWithDetailsAsync(doctorId);
                if (doctor == null)
                {
                    return NotFound(new ApiResponse($"Doctor with ID {doctorId} not found"));
                }
            }
            catch (Exception)
            {
                return NotFound(new ApiResponse($"Doctor with ID {doctorId} not found"));
            }

            var list = await _service.GetByDoctorIdAsync(doctorId);
            return Ok(new ApiResponse("Success", list));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PatientTreatmentResponse>>> Create([FromBody] PatientTreatmentRequest request)
        {
            try
            {
                var result = await _service.CreateAsync(request);
                return CreatedAtAction(nameof(GetById), new { id = result.PatientTreatmentId }, new ApiResponse("Created", result));
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<PatientTreatmentResponse>>> Update(int id, [FromBody] PatientTreatmentRequest request)
        {
            try
            {
                var result = await _service.UpdateAsync(id, request);
                return Ok(new ApiResponse("Updated", result));
            }
            catch (KeyNotFoundException)
            {
                return NotFound(new ApiResponse("Treatment not found"));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse>> Delete(int id)
        {
            var ok = await _service.DeleteAsync(id);
            if (!ok) return NotFound(new ApiResponse("Treatment not found"));
            return Ok(new ApiResponse("Deleted"));
        }
    }
} 