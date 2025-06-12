using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Controller for managing Standard ARV Regimens
    /// </summary>
    [Route("api/standard-arv-regimens")]
    [ApiController]
    // [Authorize]
    public class StandardARVRegimenController : ControllerBase
    {
        private readonly IStandardARVRegimenService _regimenService;

        public StandardARVRegimenController(IStandardARVRegimenService regimenService)
        {
            _regimenService = regimenService;
        }

        /// <summary>
        /// Get all standard ARV regimens
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var regimens = await _regimenService.GetAllAsync();
            return Ok(new ApiResponse("Success", regimens));
        }

        /// <summary>
        /// Get a standard ARV regimen by ID
        /// </summary>
        /// <param name="id">The ID of the regimen to retrieve</param>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var regimen = await _regimenService.GetByIdAsync(id);
            if (regimen == null)
                return NotFound(new ApiResponse("Regimen not found."));

            return Ok(new ApiResponse("Success", regimen));
        }

        /// <summary>
        /// Create a new standard ARV regimen
        /// </summary>
        /// <param name="request">The regimen data to create</param>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] StandardARVRegimenRequest request)
        {
            try
            {
                var regimen = await _regimenService.CreateAsync(request);
                return CreatedAtAction(
                    nameof(GetById), 
                    new { id = regimen.RegimenId }, 
                    new ApiResponse("Regimen created successfully", regimen)
                );
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while creating the regimen."));
            }
        }

        /// <summary>
        /// Update an existing standard ARV regimen
        /// </summary>
        /// <param name="id">The ID of the regimen to update</param>
        /// <param name="request">The updated regimen data</param>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(int id, [FromBody] StandardARVRegimenRequest request)
        {
            try
            {
                var regimen = await _regimenService.UpdateAsync(id, request);
                return Ok(new ApiResponse("Regimen updated successfully", regimen));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while updating the regimen."));
            }
        }

        /// <summary>
        /// Delete a standard ARV regimen
        /// </summary>
        /// <param name="id">The ID of the regimen to delete</param>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await _regimenService.DeleteAsync(id);
                return Ok(new ApiResponse("Regimen deleted successfully."));
            }
            catch (ArgumentException ex)
            {
                return NotFound(new ApiResponse(ex.Message));
            }
            catch (Exception)
            {
                return StatusCode(500, new ApiResponse("An error occurred while deleting the regimen."));
            }
        }
    }
} 