using System.Globalization;
using HIVTreatmentSystem.Domain.Entities;
using HIVTreatmentSystem.Domain.Enums;
using HIVTreatmentSystem.Domain.Enums;
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

    public async Task<(IEnumerable<DashboardStatistics> Items, int TotalCount)> GetAllAsync(
        string? entity,
        string? groupBy,
        DateTime? from,
        DateTime? to
    )
    {
        var start = from ?? DateTime.MinValue;
        var end = to ?? DateTime.UtcNow;
        List<DashboardStatistics> stats;

        switch ((entity ?? "").Trim().ToLowerInvariant())
        {
            case "appointment":
                stats = await GetDateStats(
                    _context
                        .Appointments.Where(a => a.CreatedAt >= start && a.CreatedAt <= end)
                        .Select(a => a.CreatedAt),
                    groupBy
                );
                break;

            case "patient":
                stats = await GetDateStats(
                    _context
                        .Accounts.Where(a => a.CreatedAt >= start && a.CreatedAt <= end)
                        .Select(a => a.CreatedAt),
                    groupBy
                );
                break;

            case "testresult":
                stats = await GetDateStats(
                    _context
                        .TestResults.Where(tr => tr.TestDate >= start && tr.TestDate <= end)
                        .Select(tr => tr.TestDate),
                    groupBy
                );
                break;

            case "patientgender":
                stats = await GetDemographicStats(
                    _context
                        .Patients.Where(p => p.Gender.HasValue)
                        .GroupBy(p => p.Gender.Value.ToString())
                        .Select(g => new { Key = g.Key, Count = g.Count() })
                );
                break;

            case "patientage":
                stats = await GetDemographicStats(
                    await _context
                        .Patients.Where(p => p.DateOfBirth.HasValue)
                        .Select(p => DateTime.UtcNow.Year - p.DateOfBirth!.Value.Year)
                        .ToListAsync()
                        .ContinueWith(t =>
                            t.Result.GroupBy(age => GetAgeGroup(age))
                                .Select(g => new { Key = g.Key, Count = g.Count() })
                        )
                );
                break;

            default:
                throw new ArgumentException($"Unsupported entity: {entity}");
        }

        return (stats, stats.Count);
    }

    private async Task<List<DashboardStatistics>> GetDateStats(
        IQueryable<DateTime> dates,
        string? groupBy
    )
    {
        var list = await dates.ToListAsync();
        var culture = CultureInfo.CurrentCulture;
        var rule = culture.DateTimeFormat.CalendarWeekRule;
        var first = culture.DateTimeFormat.FirstDayOfWeek;

        var q = (groupBy ?? "day").Trim().ToLowerInvariant();
        IEnumerable<DashboardStatistics> result = q switch
        {
            "day" => list.GroupBy(d => d.Date)
                .Select(g => new DashboardStatistics
                {
                    Period = g.Key.ToString("yyyy-MM-dd"),
                    Date = g.Key,
                    Count = g.Count(),
                }),

            "week" => list.GroupBy(d => culture.Calendar.GetWeekOfYear(d, rule, first))
                .Select(g =>
                {
                    var wk = g.Key;
                    var yr = g.First().Year;
                    var dateOfWeek = GetFirstDateOfWeek(yr, wk, culture, rule, first);
                    return new DashboardStatistics
                    {
                        Period = $"W{wk}-{yr}",
                        Date = dateOfWeek,
                        Count = g.Count(),
                    };
                }),

            "month" => list.GroupBy(d => new { d.Year, d.Month })
                .Select(g => new DashboardStatistics
                {
                    Period = $"{g.Key.Year}-{g.Key.Month:D2}",
                    Date = new DateTime(g.Key.Year, g.Key.Month, 1),
                    Count = g.Count(),
                }),

            "year" => list.GroupBy(d => d.Year)
                .Select(g => new DashboardStatistics
                {
                    Period = g.Key.ToString(),
                    Date = new DateTime(g.Key, 1, 1),
                    Count = g.Count(),
                }),

            _ => throw new ArgumentException($"Unsupported groupBy: {groupBy}"),
        };

        return result.OrderBy(x => x.Date).ToList();
    }

    private Task<List<DashboardStatistics>> GetDemographicStats<T>(IEnumerable<T> grouped)
    {
        var list = grouped.ToList();
        var total = list.Sum(item => (int)item.GetType().GetProperty("Count")!.GetValue(item)!);

        var stats = list.Select(item =>
            {
                var key = item.GetType().GetProperty("Key")!.GetValue(item)!.ToString()!;
                var cnt = (int)item.GetType().GetProperty("Count")!.GetValue(item)!;
                return new DashboardStatistics
                {
                    Period = key,
                    Count = cnt,
                    Percentage = total > 0 ? Math.Round(cnt * 100.0 / total, 2) : 0,
                };
            })
            .ToList();

        return Task.FromResult(stats);
    }

    private static DateTime GetFirstDateOfWeek(
        int year,
        int week,
        CultureInfo culture,
        CalendarWeekRule rule,
        DayOfWeek firstDay
    )
    {
        var jan1 = new DateTime(year, 1, 1);
        var firstWeek = culture.Calendar.GetWeekOfYear(jan1, rule, firstDay);
        var daysOffset = (int)firstDay - (int)jan1.DayOfWeek;
        var firstWeekDay = jan1.AddDays(daysOffset);
        if (firstWeek <= 1)
            week -= 1;
        return firstWeekDay.AddDays(week * 7);
    }

    private static string GetAgeGroup(int age) =>
        age switch
        {
            < 18 => "0-17",
            >= 18 and < 25 => "18-24",
            >= 25 and < 35 => "25-34",
            >= 35 and < 45 => "35-44",
            >= 45 and < 55 => "45-54",
            >= 55 and < 65 => "55-64",
            _ => "65+",
        };

    public async Task<object> GetTestResultSummaryAsync()
    {
        var totalTests = await _context.TestResults.CountAsync();

        var positiveCount = await _context.TestResults.CountAsync(tr =>
            tr.TestResults != null && tr.TestResults.ToLower() == "positive"
        );

        var negativeCount = totalTests - positiveCount;

        var positivePercentage =
            totalTests > 0 ? Math.Round((double)positiveCount / totalTests * 100, 2) : 0.0;
        var negativePercentage =
            totalTests > 0 ? Math.Round((double)negativeCount / totalTests * 100, 2) : 0.0;

        return new
        {
            TotalTests = totalTests,
            PositiveCount = positiveCount,
            NegativeCount = negativeCount,
            PositivePercentage = positivePercentage,
            NegativePercentage = negativePercentage,
        };
    }

    public async Task<IEnumerable<(string Status, int Count)>> GetTreatmentStatusCountsAsync()
    {
        var list = await _context
            .PatientTreatments.AsNoTracking()
            .GroupBy(pt => pt.Status)
            .Select(g => new { Status = g.Key.ToString(), Count = g.Count() })
            .ToListAsync();

        return list.Select(x => (x.Status, x.Count));
    }
}
