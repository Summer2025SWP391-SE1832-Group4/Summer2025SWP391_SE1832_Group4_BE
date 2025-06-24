using HIVTreatmentSystem.Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IPatientService
    {
        Task<bool> UpdatePatientAsync(int patientId, UpdatePatientRequest dto);

    }
}
