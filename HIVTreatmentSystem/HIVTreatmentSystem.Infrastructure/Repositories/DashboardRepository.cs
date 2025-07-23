using HIVTreatmentSystem.Domain.Interfaces;
using HIVTreatmentSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HIVTreatmentSystem.Infrastructure.Repositories;

public class DashboardRepository : IDashboardRepository
{
    private readonly HIVDbContext _context;

    public DashboardRepository(HIVDbContext context)
    {
        _context = context;
    }

    // Appointment statistics methods
    public async Task<List<object>> GetAppointmentStatisticsByMonthAsync()
    {
        var currentYear = DateTime.UtcNow.Year;

        // Fetch data first
        var appointments = await _context.Appointments
            .Where(a => a.CreatedAt.Year == currentYear)
            .ToListAsync();

        // Group and format on client-side
        var monthlyStats = appointments
            .GroupBy(a => new { a.CreatedAt.Year, a.CreatedAt.Month })
            .Select(g => new
            {
                Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                Count = g.Count(),
                Date = new DateTime(g.Key.Year, g.Key.Month, 1)
            })
            .OrderBy(x => x.Date)
            .Cast<object>()
            .ToList();

        return monthlyStats;
    }

    public async Task<List<object>> GetAppointmentStatisticsByDayAsync()
    {
        var last30Days = DateTime.UtcNow.AddDays(-30);
        
        var dailyStats = await _context.Appointments
            .Where(a => a.CreatedAt >= last30Days)
            .GroupBy(a => a.CreatedAt.Date)
            .Select(g => new 
            {
                Period = g.Key.ToString("yyyy-MM-dd"),
                Count = g.Count(),
                Date = g.Key
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return dailyStats.Cast<object>().ToList();
    }

    public async Task<List<object>> GetAppointmentStatisticsByYearAsync()
    {
        // Fetch all appointments first
        var appointments = await _context.Appointments.ToListAsync();

        // Group and format on client-side
        var yearlyStats = appointments
            .GroupBy(a => a.CreatedAt.Year)
            .Select(g => new
            {
                Period = g.Key.ToString(),
                Count = g.Count(),
                Date = new DateTime(g.Key, 1, 1)
            })
            .OrderBy(x => x.Date)
            .Cast<object>()
            .ToList();

        return yearlyStats;
    }

    public async Task<List<object>> GetAppointmentStatisticsByWeekAsync()
    {
        var last12Weeks = DateTime.UtcNow.AddDays(-84); // 12 weeks
        
        var weeklyData = await _context.Appointments
            .Where(a => a.CreatedAt >= last12Weeks)
            .Select(a => a.CreatedAt)
            .ToListAsync();

        var groupedStats = weeklyData
            .GroupBy(date => GetWeekOfYear(date))
            .Select(g => new 
            {
                Period = $"Week {g.Key.Week} - {g.Key.Year}",
                Count = g.Count(),
                Date = GetFirstDateOfWeek(g.Key.Year, g.Key.Week)
            })
            .OrderBy(x => x.Date)
            .ToList();

        return groupedStats.Cast<object>().ToList();
    }

    // Patient statistics methods
    public async Task<List<object>> GetPatientStatisticsByMonthAsync()
    {
        var currentYear = DateTime.UtcNow.Year;

        // Fetch joined data first
        var patients = await _context.Patients
            .Join(_context.Accounts, p => p.AccountId, a => a.AccountId, (p, a) => new { p, a })
            .Where(pa => pa.a.CreatedAt.Year == currentYear)
            .ToListAsync();

        // Group and format on client-side
        var monthlyStats = patients
            .GroupBy(pa => new { pa.a.CreatedAt.Year, pa.a.CreatedAt.Month })
            .Select(g => new
            {
                Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                Count = g.Count(),
                Date = new DateTime(g.Key.Year, g.Key.Month, 1)
            })
            .OrderBy(x => x.Date)
            .Cast<object>()
            .ToList();

        return monthlyStats;
    }

    public async Task<List<object>> GetPatientStatisticsByDayAsync()
    {
        var last30Days = DateTime.UtcNow.AddDays(-30);
        
        var dailyStats = await _context.Patients
            .Join(_context.Accounts, p => p.AccountId, a => a.AccountId, (p, a) => new { p, a })
            .Where(pa => pa.a.CreatedAt >= last30Days)
            .GroupBy(pa => pa.a.CreatedAt.Date)
            .Select(g => new 
            {
                Period = g.Key.ToString("yyyy-MM-dd"),
                Count = g.Count(),
                Date = g.Key
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return dailyStats.Cast<object>().ToList();
    }

    public async Task<List<object>> GetPatientStatisticsByYearAsync()
    {
        // Fetch joined data first
        var patients = await _context.Patients
            .Join(_context.Accounts, p => p.AccountId, a => a.AccountId, (p, a) => new { p, a })
            .ToListAsync();

        // Group and format on client-side
        var yearlyStats = patients
            .GroupBy(pa => pa.a.CreatedAt.Year)
            .Select(g => new
            {
                Period = g.Key.ToString(),
                Count = g.Count(),
                Date = new DateTime(g.Key, 1, 1)
            })
            .OrderBy(x => x.Date)
            .Cast<object>()
            .ToList();

        return yearlyStats;
    }

    public async Task<List<object>> GetPatientStatisticsByWeekAsync()
    {
        var last12Weeks = DateTime.UtcNow.AddDays(-84); // 12 weeks
        
        var weeklyData = await _context.Patients
            .Join(_context.Accounts, p => p.AccountId, a => a.AccountId, (p, a) => new { p, a })
            .Where(pa => pa.a.CreatedAt >= last12Weeks)
            .Select(pa => pa.a.CreatedAt)
            .ToListAsync();

        var groupedStats = weeklyData
            .GroupBy(date => GetWeekOfYear(date))
            .Select(g => new 
            {
                Period = $"Week {g.Key.Week} - {g.Key.Year}",
                Count = g.Count(),
                Date = GetFirstDateOfWeek(g.Key.Year, g.Key.Week)
            })
            .OrderBy(x => x.Date)
            .ToList();

        return groupedStats.Cast<object>().ToList();
    }

    // Test result statistics methods
    public async Task<List<object>> GetTestResultStatisticsByMonthAsync()
    {
        var currentYear = DateTime.UtcNow.Year;

        // Fetch data first
        var testResults = await _context.TestResults
            .Where(tr => tr.TestDate.Year == currentYear)
            .ToListAsync();

        // Group and format on client-side
        var monthlyStats = testResults
            .GroupBy(tr => new { tr.TestDate.Year, tr.TestDate.Month })
            .Select(g => new
            {
                Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                Count = g.Count(),
                Date = new DateTime(g.Key.Year, g.Key.Month, 1)
            })
            .OrderBy(x => x.Date)
            .Cast<object>()
            .ToList();

        return monthlyStats;
    }

    public async Task<List<object>> GetTestResultStatisticsByDayAsync()
    {
        var last30Days = DateTime.UtcNow.AddDays(-30);
        
        var dailyStats = await _context.TestResults
            .Where(tr => tr.TestDate >= last30Days)
            .GroupBy(tr => tr.TestDate.Date)
            .Select(g => new 
            {
                Period = g.Key.ToString("yyyy-MM-dd"),
                Count = g.Count(),
                Date = g.Key
            })
            .OrderBy(x => x.Date)
            .ToListAsync();

        return dailyStats.Cast<object>().ToList();
    }

    public async Task<List<object>> GetTestResultStatisticsByYearAsync()
    {
        // Fetch all test results first
        var testResults = await _context.TestResults.ToListAsync();

        // Group and format on client-side
        var yearlyStats = testResults
            .GroupBy(tr => tr.TestDate.Year)
            .Select(g => new
            {
                Period = g.Key.ToString(),
                Count = g.Count(),
                Date = new DateTime(g.Key, 1, 1)
            })
            .OrderBy(x => x.Date)
            .Cast<object>()
            .ToList();

        return yearlyStats;
    }

    public async Task<List<object>> GetTestResultStatisticsByWeekAsync()
    {
        var last12Weeks = DateTime.UtcNow.AddDays(-84); // 12 weeks
        
        var weeklyData = await _context.TestResults
            .Where(tr => tr.TestDate >= last12Weeks)
            .Select(tr => tr.TestDate)
            .ToListAsync();

        var groupedStats = weeklyData
            .GroupBy(date => GetWeekOfYear(date))
            .Select(g => new 
            {
                Period = $"Week {g.Key.Week} - {g.Key.Year}",
                Count = g.Count(),
                Date = GetFirstDateOfWeek(g.Key.Year, g.Key.Week)
            })
            .OrderBy(x => x.Date)
            .ToList();

        return groupedStats.Cast<object>().ToList();
    }

    // Patient demographic statistics methods
    public async Task<List<object>> GetPatientGenderStatisticsAsync()
    {
        var totalPatients = await _context.Patients.CountAsync();
        
        if (totalPatients == 0)
        {
            return new List<object>();
        }

        var genderStats = await _context.Patients
            .Where(p => p.Gender.HasValue)
            .GroupBy(p => p.Gender)
            .Select(g => new 
            {
                Gender = g.Key!.Value.ToString(),
                Count = g.Count(),
                Percentage = Math.Round((double)g.Count() / totalPatients * 100, 2)
            })
            .ToListAsync();

        return genderStats.Cast<object>().ToList();
    }

    public async Task<List<object>> GetPatientAgeStatisticsAsync()
    {
        var totalPatients = await _context.Patients.Where(p => p.DateOfBirth.HasValue).CountAsync();
        
        if (totalPatients == 0)
        {
            return new List<object>();
        }

        var currentDate = DateTime.UtcNow;
        var patientsWithAge = await _context.Patients
            .Where(p => p.DateOfBirth.HasValue)
            .Select(p => new { Patient = p, Age = currentDate.Year - p.DateOfBirth!.Value.Year })
            .ToListAsync();

        var ageGroups = patientsWithAge
            .GroupBy(p => GetAgeGroup(p.Age))
            .Select(g => new 
            {
                AgeRange = g.Key,
                Count = g.Count(),
                Percentage = Math.Round((double)g.Count() / totalPatients * 100, 2)
            })
            .OrderBy(x => x.AgeRange)
            .ToList();

        return ageGroups.Cast<object>().ToList();
    }

    public async Task<List<object>> GetPatientPregnancyStatisticsAsync()
    {
        // This would require additional fields in Patient entity for pregnancy status
        // For now, returning empty list as placeholder
        var femalePatients = await _context.Patients
            .Where(p => p.Gender == Domain.Enums.Gender.Female)
            .CountAsync();

        var pregnancyStats = new List<object>
        {
            new 
            {
                PregnancyStatus = "Not Applicable (Male/Other)",
                Count = await _context.Patients.Where(p => p.Gender != Domain.Enums.Gender.Female).CountAsync(),
                Percentage = 0.0
            },
            new 
            {
                PregnancyStatus = "Female Patients",
                Count = femalePatients,
                Percentage = 100.0
            }
        };

        var totalPatients = await _context.Patients.CountAsync();
        if (totalPatients > 0)
        {
            var maleOtherCount = await _context.Patients.Where(p => p.Gender != Domain.Enums.Gender.Female).CountAsync();
            pregnancyStats = new List<object>
            {
                new 
                {
                    PregnancyStatus = "Not Applicable (Male/Other)",
                    Count = maleOtherCount,
                    Percentage = Math.Round((double)maleOtherCount / totalPatients * 100, 2)
                },
                new 
                {
                    PregnancyStatus = "Female Patients",
                    Count = femalePatients,
                    Percentage = Math.Round((double)femalePatients / totalPatients * 100, 2)
                }
            };
        }

        return pregnancyStats;
    }

    public async Task<List<object>> GetPatientTreatmentStatusStatisticsAsync()
    {
        var totalTreatments = await _context.PatientTreatments.CountAsync();
        
        if (totalTreatments == 0)
        {
            return new List<object>();
        }

        var treatmentStats = await _context.PatientTreatments
            .GroupBy(pt => pt.Status)
            .Select(g => new 
            {
                TreatmentStatus = g.Key.ToString(),
                Count = g.Count(),
                Percentage = Math.Round((double)g.Count() / totalTreatments * 100, 2)
            })
            .ToListAsync();

        return treatmentStats.Cast<object>().ToList();
    }

    // Helper methods
    private static (int Year, int Week) GetWeekOfYear(DateTime date)
    {
        var culture = System.Globalization.CultureInfo.CurrentCulture;
        var calendar = culture.Calendar;
        var weekOfYear = calendar.GetWeekOfYear(date, culture.DateTimeFormat.CalendarWeekRule, culture.DateTimeFormat.FirstDayOfWeek);
        return (date.Year, weekOfYear);
    }

    private static DateTime GetFirstDateOfWeek(int year, int weekOfYear)
    {
        var jan1 = new DateTime(year, 1, 1);
        var daysOffset = (int)System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek - (int)jan1.DayOfWeek;
        var firstWeekDay = jan1.AddDays(daysOffset);
        var firstWeek = System.Globalization.CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(jan1, System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.CalendarWeekRule, System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.FirstDayOfWeek);
        
        if (firstWeek <= 1)
        {
            weekOfYear -= 1;
        }
        
        return firstWeekDay.AddDays(weekOfYear * 7);
    }

    private static string GetAgeGroup(int age)
    {
        return age switch
        {
            < 18 => "0-17",
            >= 18 and < 25 => "18-24",
            >= 25 and < 35 => "25-34",
            >= 35 and < 45 => "35-44",
            >= 45 and < 55 => "45-54",
            >= 55 and < 65 => "55-64",
            >= 65 => "65+"
        };
    }
}
