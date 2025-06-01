using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace HIVTreatmentSystem.Domain.Entities
{
    public class BlogTag
    {
        [Key]
        public int BlogTagId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(255)]
        public string Description { get; set; }

        public ICollection<Blog> Blogs { get; set; }
    }
}