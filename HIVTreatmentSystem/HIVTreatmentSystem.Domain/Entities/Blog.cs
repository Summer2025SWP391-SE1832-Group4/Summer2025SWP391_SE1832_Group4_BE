using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Common;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Blog : BaseEntity
    {
        public int AuthorId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; }

        [StringLength(500)]
        public string Summary { get; set; }

        [Required]
        public string Content { get; set; }

        public string FeaturedImageUrl { get; set; }

        public BlogStatus Status { get; set; }

        public DateTime? PublishedDate { get; set; }

        [StringLength(200)]
        public string Tags { get; set; }

        [StringLength(100)]
        public string Category { get; set; }

        public int ViewCount { get; set; } = 0;

        [StringLength(200)]
        public string MetaDescription { get; set; }

        [StringLength(200)]
        public string MetaKeywords { get; set; }

        // Navigation Properties
        [ForeignKey("AuthorId")]
        public virtual User Author { get; set; }
    }
}
