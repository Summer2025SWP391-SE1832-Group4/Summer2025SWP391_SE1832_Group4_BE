using HIVTreatmentSystem.Application.Interfaces;
using HIVTreatmentSystem.Application.Models.DashBroad;
using HIVTreatmentSystem.Domain.Interfaces;
using System.Dynamic;

namespace HIVTreatmentSystem.Application.Services.DashBroadService;

public class DashboardService : IDashboardService
{
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardService(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    // Monthly statistics
    public async Task<List<AppointmentStatisticsResponse>> GetAppointmentStatisticsByMonthAsync()
    {
        var data = await _dashboardRepository.GetAppointmentStatisticsByMonthAsync();
        return MapToAppointmentStatistics(data);
    }

    public async Task<List<PatientStatisticsResponse>> GetPatientStatisticsByMonthAsync()
    {
        var data = await _dashboardRepository.GetPatientStatisticsByMonthAsync();
        return MapToPatientStatistics(data);
    }

    public async Task<List<TestResultStatisticsResponse>> GetTestResultStatisticsByMonthAsync()
    {
        var data = await _dashboardRepository.GetTestResultStatisticsByMonthAsync();
        return MapToTestResultStatistics(data);
    }

    // Daily statistics
    public async Task<List<AppointmentStatisticsResponse>> GetAppointmentStatisticsByDayAsync()
    {
        var data = await _dashboardRepository.GetAppointmentStatisticsByDayAsync();
        return MapToAppointmentStatistics(data);
    }

    public async Task<List<PatientStatisticsResponse>> GetPatientStatisticsByDayAsync()
    {
        var data = await _dashboardRepository.GetPatientStatisticsByDayAsync();
        return MapToPatientStatistics(data);
    }

    public async Task<List<TestResultStatisticsResponse>> GetTestResultStatisticsByDayAsync()
    {
        var data = await _dashboardRepository.GetTestResultStatisticsByDayAsync();
        return MapToTestResultStatistics(data);
    }

    // Yearly statistics
    public async Task<List<AppointmentStatisticsResponse>> GetAppointmentStatisticsByYearAsync()
    {
        var data = await _dashboardRepository.GetAppointmentStatisticsByYearAsync();
        return MapToAppointmentStatistics(data);
    }

    public async Task<List<PatientStatisticsResponse>> GetPatientStatisticsByYearAsync()
    {
        var data = await _dashboardRepository.GetPatientStatisticsByYearAsync();
        return MapToPatientStatistics(data);
    }

    public async Task<List<TestResultStatisticsResponse>> GetTestResultStatisticsByYearAsync()
    {
        var data = await _dashboardRepository.GetTestResultStatisticsByYearAsync();
        return MapToTestResultStatistics(data);
    }

    // Weekly statistics
    public async Task<List<AppointmentStatisticsResponse>> GetAppointmentStatisticsByWeekAsync()
    {
        var data = await _dashboardRepository.GetAppointmentStatisticsByWeekAsync();
        return MapToAppointmentStatistics(data);
    }

    public async Task<List<PatientStatisticsResponse>> GetPatientStatisticsByWeekAsync()
    {
        var data = await _dashboardRepository.GetPatientStatisticsByWeekAsync();
        return MapToPatientStatistics(data);
    }

    public async Task<List<TestResultStatisticsResponse>> GetTestResultStatisticsByWeekAsync()
    {
        var data = await _dashboardRepository.GetTestResultStatisticsByWeekAsync();
        return MapToTestResultStatistics(data);
    }

    // Patient demographic statistics
    public async Task<List<PatientGenderStatisticsResponse>> GetPatientGenderStatisticsAsync()
    {
        var data = await _dashboardRepository.GetPatientGenderStatisticsAsync();
        return MapToGenderStatistics(data);
    }

    public async Task<List<PatientAgeStatisticsResponse>> GetPatientAgeStatisticsAsync()
    {
        var data = await _dashboardRepository.GetPatientAgeStatisticsAsync();
        return MapToAgeStatistics(data);
    }

    public async Task<List<PatientPregnancyStatisticsResponse>> GetPatientPregnancyStatisticsAsync()
    {
        var data = await _dashboardRepository.GetPatientPregnancyStatisticsAsync();
        return MapToPregnancyStatistics(data);
    }

    public async Task<List<PatientTreatmentStatusStatisticsResponse>> GetPatientTreatmentStatusStatisticsAsync()
    {
        var data = await _dashboardRepository.GetPatientTreatmentStatusStatisticsAsync();
        return MapToTreatmentStatusStatistics(data);
    }

    // Private mapping methods
    private List<AppointmentStatisticsResponse> MapToAppointmentStatistics(List<object> data)
    {
        return data.Select(item =>
        {
            var props = GetObjectProperties(item);
            return new AppointmentStatisticsResponse
            {
                Period = props.TryGetValue("Period", out var period) ? period.ToString() ?? "" : "",
                Count = props.TryGetValue("Count", out var count) ? Convert.ToInt32(count) : 0,
                Date = props.TryGetValue("Date", out var date) ? Convert.ToDateTime(date) : DateTime.MinValue
            };
        }).ToList();
    }

    private List<PatientStatisticsResponse> MapToPatientStatistics(List<object> data)
    {
        return data.Select(item =>
        {
            var props = GetObjectProperties(item);
            return new PatientStatisticsResponse
            {
                Period = props.TryGetValue("Period", out var period) ? period.ToString() ?? "" : "",
                Count = props.TryGetValue("Count", out var count) ? Convert.ToInt32(count) : 0,
                Date = props.TryGetValue("Date", out var date) ? Convert.ToDateTime(date) : DateTime.MinValue
            };
        }).ToList();
    }

    private List<TestResultStatisticsResponse> MapToTestResultStatistics(List<object> data)
    {
        return data.Select(item =>
        {
            var props = GetObjectProperties(item);
            return new TestResultStatisticsResponse
            {
                Period = props.TryGetValue("Period", out var period) ? period.ToString() ?? "" : "",
                Count = props.TryGetValue("Count", out var count) ? Convert.ToInt32(count) : 0,
                Date = props.TryGetValue("Date", out var date) ? Convert.ToDateTime(date) : DateTime.MinValue
            };
        }).ToList();
    }

    private List<PatientGenderStatisticsResponse> MapToGenderStatistics(List<object> data)
    {
        return data.Select(item =>
        {
            var props = GetObjectProperties(item);
            return new PatientGenderStatisticsResponse
            {
                Gender = props.TryGetValue("Gender", out var gender) ? gender.ToString() ?? "" : "",
                Count = props.TryGetValue("Count", out var count) ? Convert.ToInt32(count) : 0,
                Percentage = props.TryGetValue("Percentage", out var percentage) ? Convert.ToDouble(percentage) : 0.0
            };
        }).ToList();
    }

    private List<PatientAgeStatisticsResponse> MapToAgeStatistics(List<object> data)
    {
        return data.Select(item =>
        {
            var props = GetObjectProperties(item);
            return new PatientAgeStatisticsResponse
            {
                AgeRange = props.TryGetValue("AgeRange", out var ageRange) ? ageRange.ToString() ?? "" : "",
                Count = props.TryGetValue("Count", out var count) ? Convert.ToInt32(count) : 0,
                Percentage = props.TryGetValue("Percentage", out var percentage) ? Convert.ToDouble(percentage) : 0.0
            };
        }).ToList();
    }

    private List<PatientPregnancyStatisticsResponse> MapToPregnancyStatistics(List<object> data)
    {
        return data.Select(item =>
        {
            var props = GetObjectProperties(item);
            return new PatientPregnancyStatisticsResponse
            {
                PregnancyStatus = props.TryGetValue("PregnancyStatus", out var status) ? status.ToString() ?? "" : "",
                Count = props.TryGetValue("Count", out var count) ? Convert.ToInt32(count) : 0,
                Percentage = props.TryGetValue("Percentage", out var percentage) ? Convert.ToDouble(percentage) : 0.0
            };
        }).ToList();
    }

    private List<PatientTreatmentStatusStatisticsResponse> MapToTreatmentStatusStatistics(List<object> data)
    {
        return data.Select(item =>
        {
            var props = GetObjectProperties(item);
            return new PatientTreatmentStatusStatisticsResponse
            {
                TreatmentStatus = props.TryGetValue("TreatmentStatus", out var status) ? status.ToString() ?? "" : "",
                Count = props.TryGetValue("Count", out var count) ? Convert.ToInt32(count) : 0,
                Percentage = props.TryGetValue("Percentage", out var percentage) ? Convert.ToDouble(percentage) : 0.0
            };
        }).ToList();
    }

    private Dictionary<string, object> GetObjectProperties(object obj)
    {
        var properties = new Dictionary<string, object>();
        var type = obj.GetType();
        
        foreach (var prop in type.GetProperties())
        {
            var value = prop.GetValue(obj);
            if (value != null)
            {
                properties[prop.Name] = value;
            }
        }
        
        return properties;
    }
}