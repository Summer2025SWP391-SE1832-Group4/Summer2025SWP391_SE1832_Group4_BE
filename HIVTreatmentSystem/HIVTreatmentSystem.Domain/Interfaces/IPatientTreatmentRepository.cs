using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IPatientTreatmentRepository : IGenericRepository<PatientTreatment, int>
    {
        Task<IEnumerable<PatientTreatment>> GetByPatientIdAsync(int patientId);
    }
}
