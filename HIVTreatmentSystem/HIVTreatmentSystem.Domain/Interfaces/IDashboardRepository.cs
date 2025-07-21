namespace HIVTreatmentSystem.Domain.Interfaces;

public interface IDashboardRepository
{
    // Appointment statistics - các method trả về anonymous type được convert sang response trong service layer
    Task<List<object>> GetAppointmentStatisticsByMonthAsync();
    Task<List<object>> GetAppointmentStatisticsByDayAsync();
    Task<List<object>> GetAppointmentStatisticsByYearAsync();
    Task<List<object>> GetAppointmentStatisticsByWeekAsync();
    
    // Patient statistics
    Task<List<object>> GetPatientStatisticsByMonthAsync();
    Task<List<object>> GetPatientStatisticsByDayAsync();
    Task<List<object>> GetPatientStatisticsByYearAsync();
    Task<List<object>> GetPatientStatisticsByWeekAsync();
    
    // Test result statistics
    Task<List<object>> GetTestResultStatisticsByMonthAsync();
    Task<List<object>> GetTestResultStatisticsByDayAsync();
    Task<List<object>> GetTestResultStatisticsByYearAsync();
    Task<List<object>> GetTestResultStatisticsByWeekAsync();
    
    // Patient demographic statistics
    Task<List<object>> GetPatientGenderStatisticsAsync();
    Task<List<object>> GetPatientAgeStatisticsAsync();
    Task<List<object>> GetPatientPregnancyStatisticsAsync();
    Task<List<object>> GetPatientTreatmentStatusStatisticsAsync();
}
