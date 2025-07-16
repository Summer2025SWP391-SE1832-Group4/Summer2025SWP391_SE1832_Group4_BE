
using System.ComponentModel.DataAnnotations;


namespace HIVTreatmentSystem.Domain.Entities
{
    public class Role
    {
        public int RoleId { get; set; }

        [Required]
        [MaxLength(50)]
        public string RoleName { get; set; } = string.Empty;

        [MaxLength(255)]
        public string? Description { get; set; }

        // Navigation properties
        public virtual ICollection<Account> Users { get; set; } = new List<Account>();
    }
}
