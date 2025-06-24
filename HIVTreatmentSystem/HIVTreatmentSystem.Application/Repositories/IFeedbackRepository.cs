using HIVTreatmentSystem.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Repositories
{
    /// <summary>
    /// Repository interface for managing feedback records
    /// </summary>
    public interface IFeedbackRepository
    {
        /// <summary>
        /// Get paged list of feedbacks with optional filters
        /// </summary>
        /// <param name="patientId">Filter by patient id</param>
        /// <param name="appointmentId">Filter by appointment id</param>
        /// <param name="rating">Filter by rating</param>
        /// <param name="pageNumber">Page number (1-based)</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Tuple of items and total count</returns>
        Task<(IEnumerable<Feedback> Items, int TotalCount)> GetPagedAsync(
            int? patientId,
            int? appointmentId,
            int? rating,
            int pageNumber,
            int pageSize);

        /// <summary>
        /// Get a feedback by its id
        /// </summary>
        Task<Feedback?> GetByIdAsync(int id);

        /// <summary>
        /// Add new feedback
        /// </summary>
        Task<Feedback> AddAsync(Feedback feedback);

        /// <summary>
        /// Update existing feedback
        /// </summary>
        Task<Feedback> UpdateAsync(Feedback feedback);

        /// <summary>
        /// Delete feedback by id
        /// </summary>
        Task<bool> DeleteAsync(int id);
    }
} 