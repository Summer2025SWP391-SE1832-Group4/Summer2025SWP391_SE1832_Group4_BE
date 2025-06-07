using HIVTreatmentSystem.Application.Common;
using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    [Route("api/blogs")]
    [ApiController]
    public class BlogsController : ControllerBase
    {
        private readonly IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(
            [FromQuery] string? title,
            [FromQuery] string? content,
            [FromQuery] int? tagId,
            [FromQuery] DateTime? createdFrom,
            [FromQuery] DateTime? createdTo,
            [FromQuery] string? sortBy,
            [FromQuery] bool sortDesc = false,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 20
        )
        {
            var (items, total) = await _blogService.GetAllAsync(
                title,
                content,
                tagId,
                createdFrom,
                createdTo,
                sortBy,
                sortDesc,
                pageNumber,
                pageSize
            );

            return Ok(
                new ApiResponse
                {
                    Success = true,
                    Message = "Danh sách blog",
                    Data = new { Items = items, TotalCount = total },
                }
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                var dto = await _blogService.GetByIdAsync(id);
                return Ok(
                    new ApiResponse
                    {
                        Success = true,
                        Message = "Blog retrieved successfully",
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
                        Message = "không tìm thấy blog",
                        Data = null,
                    }
                );
            }
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BlogRequest req)
        {
            var dto = await _blogService.CreateAsync(req);
            return CreatedAtAction(
                nameof(GetById),
                new { id = dto.BlogId },
                new ApiResponse
                {
                    Success = true,
                    Message = "Blog created successfully",
                    Data = dto,
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] BlogRequest req)
        {
            try
            {
                var dto = await _blogService.UpdateAsync(id, req);
                return Ok(
                    new ApiResponse
                    {
                        Success = true,
                        Message = "Blog updated successfully",
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
                        Message = "không tìm thấy blog",
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
                await _blogService.DeleteAsync(id);
                return Ok(
                    new ApiResponse
                    {
                        Success = true,
                        Message = "Blog deleted successfully",
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
                        Message = "không tìm thấy blog",
                        Data = null,
                    }
                );
            }
        }
    }
}
