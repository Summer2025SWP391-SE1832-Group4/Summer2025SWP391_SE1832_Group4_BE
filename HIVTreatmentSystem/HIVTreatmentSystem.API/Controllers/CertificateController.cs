using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [Route("api/certificates")]
    [ApiController]
    public class CertificateController : ControllerBase
    {
        private readonly ICertificateService _certificateService;

        public CertificateController(ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            string? title,
            string? issuedBy,
            string? doctorName,
            DateTime? startDate,
            DateTime? endDate,
            bool isDescending = false,
            string sortBy = "title",
            int pageIndex = 1,
            int pageSize = 10)
        {
            var result = await _certificateService.GetAllCertificatesAsync(
                title, issuedBy, doctorName, startDate, endDate, isDescending, sortBy, pageIndex, pageSize);

            return Ok(result);
        }
    

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var certificate = await _certificateService.GetCertificateByIdAsync(id);

            if (certificate == null)
                return NotFound();

            return Ok(certificate);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CertificateRequest request)
        {
            try
            {
                var result = await _certificateService.CreateCertificateAsync(request);
                return Ok(result);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "Something went wrong on the server." });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] CertificateRequest request)
        {

            try
            {
                var success = await _certificateService.UpdateCertificateAsync(id, request);
                return Ok(new ApiResponse("Certificate updated successfully."));
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while updating the certificate." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _certificateService.DeleteCertificateAsync(id);
                return Ok(new ApiResponse("Certificate deleted successfully."));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }



    }



}
