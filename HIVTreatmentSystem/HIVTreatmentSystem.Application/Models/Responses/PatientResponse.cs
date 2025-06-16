using HIVTreatmentSystem.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Models.Responses
{
    public class PatientResponse
    {
        public string? PatientCodeAtFacility { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public Gender? Gender { get; set; }

        public string? Address { get; set; }

        public DateTime? HivDiagnosisDate { get; set; }

        public string? ConsentInformation { get; set; }

        public string? AnonymousIdentifier { get; set; }

        public string? AdditionalNotes { get; set; }

        public AccountResponse? Account { get; set; }

    }
}
