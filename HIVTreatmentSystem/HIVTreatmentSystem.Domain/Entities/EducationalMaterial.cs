using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class EducationalMaterial
    {
        public int MaterialId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } = string.Empty;

        [Required]
        public string Content { get; set; } = string.Empty;

        public EducationalMaterialType? MaterialType { get; set; }

        public int? AuthorId { get; set; }

        public DateTime PublishedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string? Category { get; set; }

        [MaxLength(100)]
        public string? TargetAudience { get; set; }

        public int ViewCount { get; set; } = 0;

        public bool IsPublished { get; set; } = true;

        // Navigation properties
        public virtual Account? Author { get; set; }
    }
}
