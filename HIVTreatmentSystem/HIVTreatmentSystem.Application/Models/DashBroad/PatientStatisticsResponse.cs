namespace HIVTreatmentSystem.Application.Models.DashBroad;

public class PatientStatisticsResponse
{
    public string Period { get; set; } = string.Empty;
    public int Count { get; set; }
    public DateTime Date { get; set; }
}
