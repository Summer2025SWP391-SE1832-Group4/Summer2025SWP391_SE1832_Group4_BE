using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HIVTreatmentSystem.Domain.Entities
{
	public class Certificate
    {
        [Key]
        public int CertificateId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; } 

        [MaxLength(500)]
        public string Description { get; set; }

        public DateTime IssuedDate { get; set; }

        public string IssuedBy { get; set; }

       
        public int DoctorId { get; set; }

        [ForeignKey("DoctorId")]
        public Doctor Doctor { get; set; }
    }
}
