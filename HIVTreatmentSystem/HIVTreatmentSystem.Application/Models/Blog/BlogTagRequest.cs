
using System.ComponentModel.DataAnnotations;


namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class BlogTagRequest
    {
        [Required, MaxLength(100)]
        public string Name { get; set; } = default!;

        [MaxLength(255)]
        public string? Description { get; set; }
    }
}
