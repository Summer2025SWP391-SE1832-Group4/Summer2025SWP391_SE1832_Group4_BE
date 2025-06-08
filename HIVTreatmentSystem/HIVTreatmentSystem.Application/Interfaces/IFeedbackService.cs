using HIVTreatmentSystem.Application.Models.Requests;
using HIVTreatmentSystem.Application.Models.Responses;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IFeedbackService
    {
        Task<ListFeedbacksResponse> GetPagedAsync(ListFeedbacksRequest request);
        Task<FeedbackResponse> GetByIdAsync(int id);
        Task<FeedbackResponse> CreateAsync(FeedbackRequest request);
        Task<FeedbackResponse> UpdateAsync(int id, FeedbackRequest request);
        Task DeleteAsync(int id);
    }
} 