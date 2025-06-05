using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HIVTreatmentSystem.Domain.Entities;

namespace HIVTreatmentSystem.Domain.Interfaces
{
    public interface IStaffRepository : IGenericRepository<Staff, int>
    {
        // Có thể bổ sung các method đặc thù cho Staff nếu cần
    }
} 