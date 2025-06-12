using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    /// <summary>
    /// Repository interface for Standard ARV Regimen operations
    /// </summary>
    public interface IStandardARVRegimenRepository
    {
        /// <summary>
        /// Get all standard ARV regimens
        /// </summary>
        Task<IEnumerable<StandardARVRegimen>> GetAllAsync();

        /// <summary>
        /// Get a standard ARV regimen by its ID
        /// </summary>
        /// <param name="id">The ID of the regimen to retrieve</param>
        Task<StandardARVRegimen?> GetByIdAsync(int id);

        /// <summary>
        /// Create a new standard ARV regimen
        /// </summary>
        /// <param name="regimen">The regimen to create</param>
        Task<StandardARVRegimen> CreateAsync(StandardARVRegimen regimen);

        /// <summary>
        /// Update an existing standard ARV regimen
        /// </summary>
        /// <param name="regimen">The regimen to update</param>
        Task<bool> UpdateAsync(StandardARVRegimen regimen);

        /// <summary>
        /// Delete a standard ARV regimen
        /// </summary>
        /// <param name="regimen">The regimen to delete</param>
        Task<bool> DeleteAsync(StandardARVRegimen regimen);
    }
} 