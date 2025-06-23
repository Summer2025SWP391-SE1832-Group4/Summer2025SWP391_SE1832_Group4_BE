using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;
using HIVTreatmentSystem.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HIVTreatmentSystem.API.Controllers
{
    /// <summary>
    /// Controller for managing feedback operations
    /// </summary>
    [ApiController]
    [Route("api/feedback")]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;
        private readonly IAppointmentRepository _appointmentRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public FeedbackController(IFeedbackService feedbackService, IAppointmentRepository appointmentRepository, IHttpContextAccessor httpContextAccessor)
        {
            _feedbackService = feedbackService;
            _appointmentRepository = appointmentRepository;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Get a paginated list of feedbacks with optional filtering
        /// </summary>
        /// <param name="request">Pagination and filter parameters</param>
        /// <returns>List of feedbacks and total count</returns>
        /// <response code="200">Returns the list of feedbacks</response>
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetFeedbacksAsync([FromQuery] ListFeedbacksRequest request)
        {
            ListFeedbacksResponse response = await _feedbackService.GetPagedAsync(request);

            var apiDtos = response
                .Feedbacks.Select(f => new FeedbackResponse
                {
                    FeedbackId = f.FeedbackId,
                    AppointmentId = f.AppointmentId,
                    PatientId = f.PatientId,
                    Rating = f.Rating,
                    Comment = f.Comment,
                    CreatedAt = f.CreatedAt
                })
                .ToList();

            return Ok(new { Feedbacks = apiDtos, TotalCount = response.TotalCount });
        }

        /// <summary>
        /// Get a specific feedback by its ID
        /// </summary>
        /// <param name="id">The ID of the feedback</param>
        /// <returns>The feedback details</returns>
        /// <response code="200">Returns the requested feedback</response>
        /// <response code="404">If the feedback is not found</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetFeedbackByIdAsync(int id)
        {
            var f = await _feedbackService.GetByIdAsync(id);
            var apiDto = new FeedbackResponse
            {
                FeedbackId = f.FeedbackId,
                AppointmentId = f.AppointmentId,
                PatientId = f.PatientId,
                Rating = f.Rating,
                Comment = f.Comment,
                CreatedAt = f.CreatedAt
            };
            return Ok(apiDto);
        }

        /// <summary>
        /// Create a new feedback
        /// </summary>
        /// <param name="apiRequest">The feedback data</param>
        /// <returns>The created feedback</returns>
        /// <response code="201">Returns the newly created feedback</response>
        /// <response code="400">If the request data is invalid</response>
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateAsync([FromBody] FeedbackCreateRequest apiRequest)
        {
            // Lấy appointment để suy ra PatientId
            var appointment = await _appointmentRepository.GetAppointmentWithDetailsAsync(apiRequest.AppointmentId);
            if (appointment == null)
            {
                return BadRequest(new { message = "Invalid AppointmentId" });
            }

            var appRequest = new FeedbackRequest
            {
                AppointmentId = apiRequest.AppointmentId,
                PatientId = appointment.PatientId,
                Rating = apiRequest.Rating,
                Comment = apiRequest.Comment
            };

            var f = await _feedbackService.CreateAsync(appRequest);

            var apiDto = new FeedbackResponse
            {
                FeedbackId = f.FeedbackId,
                AppointmentId = f.AppointmentId,
                PatientId = f.PatientId,
                Rating = f.Rating,
                Comment = f.Comment,
                CreatedAt = f.CreatedAt
            };

            return CreatedAtAction(nameof(GetFeedbackByIdAsync), new { id = f.FeedbackId }, apiDto);
        }

        /// <summary>
        /// Update an existing feedback
        /// </summary>
        /// <param name="id">The ID of the feedback to update</param>
        /// <param name="apiRequest">The updated feedback data</param>
        /// <returns>The updated feedback</returns>
        /// <response code="200">Returns the updated feedback</response>
        /// <response code="400">If the request data is invalid</response>
        /// <response code="404">If the feedback is not found</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> UpdateAsync(int id, [FromBody] FeedbackRequest apiRequest)
        {
            var appRequest = new FeedbackRequest
            {
                AppointmentId = apiRequest.AppointmentId,
                PatientId = apiRequest.PatientId,
                Rating = apiRequest.Rating,
                Comment = apiRequest.Comment
            };

            var f = await _feedbackService.UpdateAsync(id, appRequest);

            var apiDto = new FeedbackResponse
            {
                FeedbackId = f.FeedbackId,
                AppointmentId = f.AppointmentId,
                PatientId = f.PatientId,
                Rating = f.Rating,
                Comment = f.Comment,
                CreatedAt = f.CreatedAt
            };

            return Ok(apiDto);
        }

        /// <summary>
        /// Delete a feedback
        /// </summary>
        /// <param name="id">The ID of the feedback to delete</param>
        /// <returns>No content</returns>
        /// <response code="204">If the feedback was successfully deleted</response>
        /// <response code="404">If the feedback is not found</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _feedbackService.DeleteAsync(id);
            return NoContent();
        }
    }
} 