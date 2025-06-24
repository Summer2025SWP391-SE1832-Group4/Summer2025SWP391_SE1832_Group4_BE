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

        public PatientTreatmentController(IPatientTreatmentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientTreatmentResponse>>>> GetAll()
        {
            var list = await _service.GetAllAsync();
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
            var list = await _service.GetByPatientIdAsync(patientId);
            return Ok(new ApiResponse("Success", list));
        }

        [HttpGet("doctor/{doctorId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<PatientTreatmentResponse>>>> GetByDoctor(int doctorId)
        {
            var list = await _service.GetByDoctorIdAsync(doctorId);
            return Ok(new ApiResponse("Success", list));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<PatientTreatmentResponse>>> Create([FromBody] PatientTreatmentRequest request)
        {
            var result = await _service.CreateAsync(request);
            return CreatedAtAction(nameof(GetById), new { id = result.PatientTreatmentId }, new ApiResponse("Created", result));
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