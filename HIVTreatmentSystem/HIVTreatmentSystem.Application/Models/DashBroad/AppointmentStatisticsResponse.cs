namespace HIVTreatmentSystem.Application.Models.DashBroad;

public class AppointmentStatisticsResponse
{
    public string Period { get; set; } = string.Empty;
    public int Count { get; set; }
    public DateTime Date { get; set; }
}