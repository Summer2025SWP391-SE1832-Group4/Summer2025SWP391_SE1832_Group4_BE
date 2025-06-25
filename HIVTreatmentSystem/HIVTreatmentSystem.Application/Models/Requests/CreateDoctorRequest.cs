using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class CreateDoctorRequest
    {

        public string? Qualifications { get; set; }

        public int? YearsOfExperience { get; set; }

        public string? ShortDescription { get; set; }

        public int AccountId { get; set; }
    }
}
