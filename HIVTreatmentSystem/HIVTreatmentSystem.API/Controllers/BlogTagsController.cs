using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [Route("api/blogtags")]
    [ApiController]
    public class BlogTagsController : ControllerBase
    {
        private readonly IBlogTagService _blogTagService;

        public BlogTagsController(IBlogTagService blogTagService)
        {
            _blogTagService = blogTagService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? name,
            [FromQuery] string? sortBy,
            [FromQuery] bool sortDesc = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20
        )
        {
            var (items, total) = await _blogTagService.GetAllAsync(
                name,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );
            return Ok(
                new ApiResponse
                {
                    Success = true,
                    Message = "Danh sách thẻ",
                    Data = new { Items = items, TotalCount = total },
                }
            );
        }

        [HttpGet("id")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var dto = await _blogTagService.GetByIdAsync(id);
                return Ok(
                    new ApiResponse
                    {
                        Success = true,
                        Message = "Thẻ retrieved successfully",
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
                        Message = "không tìm thấy thẻ",
                        Data = null,
                    }
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogTagRequest req)
        {
            var dto = await _blogTagService.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetById),
                new { id = dto.BlogTagId },
                new ApiResponse
                {
                    Success = true,
                    Message = "Thẻ created successfully",
                    Data = dto,
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlogTagRequest req)
        {
            try
            {
                var dto = await _blogTagService.UpdateAsync(id, req);
                return Ok(
                    new ApiResponse
                    {
                        Success = true,
                        Message = "Thẻ updated successfully",
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
                        Message = "không tìm thấy thẻ",
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
                await _blogTagService.DeleteAsync(id);
                return Ok(
                    new ApiResponse
                    {
                        Success = true,
                        Message = "Thẻ deleted successfully",
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
                        Message = "không tìm thấy thẻ",
                        Data = null,
                    }
                );
            }
        }
    }
}
