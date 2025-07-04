

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class BlogResponse
    {
        public int BlogId { get; set; }
        public string Title { get; set; } = null!;
        public string Content { get; set; } = null!;
        public string? BlogImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public int BlogTagId { get; set; }
        public string BlogTagName { get; set; } = null!;
    }
}
