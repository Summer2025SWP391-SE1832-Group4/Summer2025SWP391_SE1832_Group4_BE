using System.ComponentModel.DataAnnotations;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class BlogRequest
    {
        [Required, MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        [MaxLength(255)]
        public string? BlogImageUrl { get; set; }

        [Required]
        public int BlogTagId { get; set; }
    }
}
