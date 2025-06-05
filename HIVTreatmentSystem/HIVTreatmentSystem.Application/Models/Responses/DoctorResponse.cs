using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class DoctorResponse
    {
        public int DoctorId { get; set; } 
        public string FullName { get; set; }

        public string? Specialty { get; set; }

        public string? Qualifications { get; set; }

        public int? YearsOfExperience { get; set; }

        public string? ShortDescription { get; set; }
    }
}
