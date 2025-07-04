using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IScheduledActivityService
    {
        Task<IEnumerable<ScheduledActivityResponse>> GetAllAsync(
            int? patientId,
            string? activityType
        );
        Task<ScheduledActivityResponse> GetByIdAsync(int id);
        Task<ScheduledActivityResponse> CreateAsync(ScheduledActivityRequest request);
        Task<ScheduledActivityResponse> UpdateAsync(int id, ScheduledActivityRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
