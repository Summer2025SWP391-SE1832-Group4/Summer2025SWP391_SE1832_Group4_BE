using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;

namespace HIVTreatmentSystem.Infrastructure.Repositories
{
    public class StaffRepository : GenericRepository<Staff, int>, IStaffRepository
    {
        public StaffRepository(HIVDbContext context) : base(context)
        {
        }
        
        // Có thể bổ sung các method đặc thù cho Staff nếu cần
    }
} 