
using System.ComponentModel.DataAnnotations;


namespace HIVTreatmentSystem.Domain.Entities
{
    public class ExperienceWorking

    {
        [Key]
        public int Id { get; set; }
        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
        public string HospitalName { get; set; }
        public string Position { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime? ToDate { get; set; }
    }
}