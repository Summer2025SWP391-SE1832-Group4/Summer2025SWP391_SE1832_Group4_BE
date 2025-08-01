﻿using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [Route("api/standard-arv-regimens")]
    [ApiController]
    public class StandardARVRegimensController : ControllerBase
    {
        private readonly IStandardARVRegimenService _standardARVRegimenService;

        public StandardARVRegimensController(IStandardARVRegimenService standardARVRegimenService)
        {
            _standardARVRegimenService = standardARVRegimenService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? regimenName,
            [FromQuery] string? targetPopulation,
            [FromQuery] string? sortBy,
            [FromQuery] bool sortDesc = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20
        )
        {
            var (items, total) = await _standardARVRegimenService.GetAllAsync(
                regimenName,
                targetPopulation,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse
                {
                    Success = true,
                    Message = "List regimen",
                    Data = new { Items = items, TotalCount = total },
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var dto = await _standardARVRegimenService.GetByIdAsync(id);
                return Ok(
                    new ApiResponse
                    {
                        Success = true,
                        Message = "Regimen retrieved successfully",
                        Data = dto,
                    }
                );
            }
            catch (KeyNotFoundException)
            {
                return NotFound(
                    new ApiResponse
                    {
                        Success = false,
                        Message = "Can't find regimen",
                        Data = null,
                    }
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] StandardARVRegimenRequest req)
        {
            var dto = await _standardARVRegimenService.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetById),
                new { id = dto.RegimenId },
                new ApiResponse
                {
                    Success = true,
                    Message = "Regimen created successfully",
                    Data = dto,
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] StandardARVRegimenRequest req)
        {
            try
            {
                var dto = await _standardARVRegimenService.UpdateAsync(id, req);
                return Ok(
                    new ApiResponse
                    {
                        Success = true,
                        Message = "Regimen updated successfully",
                        Data = dto,
                    }
                );
            }
            catch (KeyNotFoundException)
            {
                return NotFound(
                    new ApiResponse
                    {
                        Success = false,
                        Message = "Can't find regimen",
                        Data = null,
                    }
                );
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _standardARVRegimenService.DeleteAsync(id);
                return Ok(
                    new ApiResponse
                    {
                        Success = true,
                        Message = "Regimen deleted successfully",
                        Data = null,
                    }
                );
            }
            catch (KeyNotFoundException)
            {
                return NotFound(
                    new ApiResponse
                    {
                        Success = false,
                        Message = "Can't find regimen",
                        Data = null,
                    }
                );
            }
        }

        [HttpPost("suggest-regimens")]
        public async Task<IActionResult> SuggestRegimen([FromBody] RegimenSuggestionRequest req)
        {
            if (!int.TryParse(req.hivViralLoadValue, out var viralLoad))
                return BadRequest("Invalid viral load value");

            string level;
            int selectedRegimenId;

            if (req.cD4Count <= 350 || viralLoad <= 500)
            {
                level = "Tier 1";
                selectedRegimenId = 1;
            }
            else if (req.cD4Count <= 500 || viralLoad <= 800)
            {
                level = "Tier 2";
                selectedRegimenId = 2;
            }
            else
            {
                level = "Tier 3";
                selectedRegimenId = 3;
            }

            var regimen = await _standardARVRegimenService.GetByIdAsync(selectedRegimenId);
            return Ok(
                new RegimenSuggestionResponse
                {
                    RegimenId = regimen.RegimenId,
                    RegimenName = regimen.RegimenName,
                    SuggestionLevel = level,
                }
            );
        }
    }
}
