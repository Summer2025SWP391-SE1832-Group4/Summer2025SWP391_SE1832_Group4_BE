using HIVTreatmentSystem.Application.Models.DashBroad;

namespace HIVTreatmentSystem.Application.Interfaces;

public interface IDashboardService
{
    // Monthly statistics
    Task<List<AppointmentStatisticsResponse>> GetAppointmentStatisticsByMonthAsync();
    Task<List<PatientStatisticsResponse>> GetPatientStatisticsByMonthAsync();
    Task<List<TestResultStatisticsResponse>> GetTestResultStatisticsByMonthAsync();
    
    // Daily statistics
    Task<List<AppointmentStatisticsResponse>> GetAppointmentStatisticsByDayAsync();
    Task<List<PatientStatisticsResponse>> GetPatientStatisticsByDayAsync();
    Task<List<TestResultStatisticsResponse>> GetTestResultStatisticsByDayAsync();
    
    // Yearly statistics
    Task<List<AppointmentStatisticsResponse>> GetAppointmentStatisticsByYearAsync();
    Task<List<PatientStatisticsResponse>> GetPatientStatisticsByYearAsync();
    Task<List<TestResultStatisticsResponse>> GetTestResultStatisticsByYearAsync();
    
    // Weekly statistics
    Task<List<AppointmentStatisticsResponse>> GetAppointmentStatisticsByWeekAsync();
    Task<List<PatientStatisticsResponse>> GetPatientStatisticsByWeekAsync();
    Task<List<TestResultStatisticsResponse>> GetTestResultStatisticsByWeekAsync();
    
    // Patient demographic statistics
    Task<List<PatientGenderStatisticsResponse>> GetPatientGenderStatisticsAsync();
    Task<List<PatientAgeStatisticsResponse>> GetPatientAgeStatisticsAsync();
    Task<List<PatientPregnancyStatisticsResponse>> GetPatientPregnancyStatisticsAsync();
    Task<List<PatientTreatmentStatusStatisticsResponse>> GetPatientTreatmentStatusStatisticsAsync();
}