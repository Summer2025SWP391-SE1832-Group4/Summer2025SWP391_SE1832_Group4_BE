using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Domain.Entities
{
    public class Staff
    {
        public int StaffId { get; set; } // Same as UserId

        [MaxLength(100)]
        public string? Position { get; set; }

        // Navigation properties
        public virtual Account Account { get; set; } = null!;
    }
}
