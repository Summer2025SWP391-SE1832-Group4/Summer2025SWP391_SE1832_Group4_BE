using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HIVTreatmentSystem.Domain.Entities
{
    public class Blog
    {
        [Key]
        public int BlogId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        public string Content { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public int BlogTagId { get; set; }

        [ForeignKey("BlogTagId")]
        public BlogTag BlogTag { get; set; }
    }
}