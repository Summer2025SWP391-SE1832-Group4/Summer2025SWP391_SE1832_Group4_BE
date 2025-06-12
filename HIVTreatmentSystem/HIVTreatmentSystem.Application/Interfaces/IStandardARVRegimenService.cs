using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    /// <summary>
    /// Service interface for Standard ARV Regimen operations
    /// </summary>
    public interface IStandardARVRegimenService
    {
        /// <summary>
        /// Get all standard ARV regimens
        /// </summary>
        Task<IEnumerable<StandardARVRegimenResponse>> GetAllAsync();

        /// <summary>
        /// Get a standard ARV regimen by its ID
        /// </summary>
        /// <param name="id">The ID of the regimen to retrieve</param>
        Task<StandardARVRegimenResponse?> GetByIdAsync(int id);

        /// <summary>
        /// Create a new standard ARV regimen
        /// </summary>
        /// <param name="request">The regimen data to create</param>
        Task<StandardARVRegimenResponse> CreateAsync(StandardARVRegimenRequest request);

        /// <summary>
        /// Update an existing standard ARV regimen
        /// </summary>
        /// <param name="id">The ID of the regimen to update</param>
        /// <param name="request">The updated regimen data</param>
        Task<StandardARVRegimenResponse> UpdateAsync(int id, StandardARVRegimenRequest request);

        /// <summary>
        /// Delete a standard ARV regimen
        /// </summary>
        /// <param name="id">The ID of the regimen to delete</param>
        Task<bool> DeleteAsync(int id);
    }
} 