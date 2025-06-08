namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class ListFeedbacksResponse
    {
        public List<FeedbackResponse> Feedbacks { get; set; }
        public int TotalCount { get; set; }
    }
} 