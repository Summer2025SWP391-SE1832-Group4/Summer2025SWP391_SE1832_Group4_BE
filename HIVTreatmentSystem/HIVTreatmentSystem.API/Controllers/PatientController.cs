using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [Route("api/patients")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _patientService;

        public PatientController(IPatientService patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllPatient(
            [FromQuery] int? accountId,
            [FromQuery] DateTime? DateOfBirth,
            [FromQuery] Gender? Gender,
            [FromQuery] string? Address,
            [FromQuery] DateTime? HivDiagnosisDate,
            [FromQuery] string? ConsentInformation,
            [FromQuery] bool isDescending = false,
            [FromQuery] string? sortBy = "",
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10
            )
        {
            var result = await _patientService.GetAllPatientsAsync(
                accountId,
                DateOfBirth,
                Gender,
                Address,
                HivDiagnosisDate,
                ConsentInformation,
                isDescending,
                sortBy,
                pageIndex,
                pageSize);

            return Ok(result);

        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetPatientById(int id)
        {
            var patient = await _patientService.GetPatientByIdAsync(id);
            return Ok(new ApiResponse("Success", patient));
        }

        [HttpGet("account/{accountId}")]
        public async Task<IActionResult> GetPatientByAccountId(int accountId)
        {
            try
            {
                var patient = await _patientService.GetPatientByAccountIdAsync(accountId);
                return Ok(new ApiResponse("Success", patient));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse(ex.Message));
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePatientRequest request)
        {
            if (request == null) return BadRequest();

            var result = await _patientService.CreatePatientAsync(request);

            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePatient(int id, [FromBody] UpdatePatientRequest dto)
        {
            var result = await _patientService.UpdatePatientAsync(id, dto);
            if (!result) return NotFound();

            return Ok("Update successful");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePatient(int id)
        {
            var result = await _patientService.DeletePatientAsync(id);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}
