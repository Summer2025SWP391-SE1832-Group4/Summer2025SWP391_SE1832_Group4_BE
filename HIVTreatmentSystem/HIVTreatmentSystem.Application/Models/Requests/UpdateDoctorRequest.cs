using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Requests
{
    public class UpdateDoctorRequest
    {
        public string? Qualifications { get; set; }

        public int? YearsOfExperience { get; set; }

        public string? ShortDescription { get; set; }

    }
}
