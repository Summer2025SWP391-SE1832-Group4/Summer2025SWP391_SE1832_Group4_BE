using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly HIVDbContext _context;

    public DashboardRepository(HIVDbContext context)
    {
        _context = context;
    }

    public async Task<int> GetTotalPatientsCountAsync()
    {
        return await _context.Patients.CountAsync();
    }

    public async Task<(int TotalTests, int PositiveCount, int NegativeCount)> GetTestResultsSummaryAsync()
    {
        // Lấy tất cả test results có kết quả
        var results = await _context.TestResults
            .Where(tr => !string.IsNullOrEmpty(tr.TestResults) || !string.IsNullOrEmpty(tr.DoctorComments))
            .ToListAsync();

        var totalTests = results.Count;
        
        // Đếm số lượng dương tính và âm tính
        var positiveCount = results.Count(r => 
            (r.TestResults != null && r.TestResults.ToLower().Contains("positive")) ||
            (r.DoctorComments != null && r.DoctorComments.ToLower().Contains("positive")));
            
        var negativeCount = results.Count(r => 
            (r.TestResults != null && r.TestResults.ToLower().Contains("negative")) ||
            (r.DoctorComments != null && r.DoctorComments.ToLower().Contains("negative")));

        // Nếu có test không rõ kết quả, thêm vào tổng số
        var unknownCount = totalTests - (positiveCount + negativeCount);
        if (unknownCount > 0)
        {
            // Log để theo dõi
            Console.WriteLine($"Warning: {unknownCount} test results have unclear status");
        }

        return (totalTests, positiveCount, negativeCount);
    }

    public async Task<List<object>> GetPatientTreatmentStatusStatisticsAsync()
    {
        // Lấy tất cả các ca điều trị
        var treatments = await _context.PatientTreatments
            .Where(t => t.Status == TreatmentStatus.InTreatment || t.Status == TreatmentStatus.Completed)
            .ToListAsync();

        var totalTreatments = treatments.Count;

        // Nhóm theo trạng thái và tính toán phần trăm
        var groupedStats = treatments
            .GroupBy(t => t.Status)
            .Select(g => new
            {
                TreatmentStatus = g.Key.ToString(),
                Count = g.Count(),
                Percentage = totalTreatments > 0 ? Math.Round((double)g.Count() / totalTreatments * 100, 2) : 0
            })
            .OrderBy(x => x.TreatmentStatus)
            .Cast<object>()
            .ToList();

        // Nếu không có dữ liệu, trả về danh sách với các trạng thái mặc định
        if (!groupedStats.Any())
        {
            return new List<object>
            {
                new { TreatmentStatus = TreatmentStatus.InTreatment.ToString(), Count = 0, Percentage = 0.0 },
                new { TreatmentStatus = TreatmentStatus.Completed.ToString(), Count = 0, Percentage = 0.0 }
            };
        }

        return groupedStats;
    }
}
