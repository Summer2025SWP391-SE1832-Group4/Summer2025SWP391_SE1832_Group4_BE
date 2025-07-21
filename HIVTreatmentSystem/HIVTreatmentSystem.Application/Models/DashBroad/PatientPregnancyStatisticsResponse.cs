namespace HIVTreatmentSystem.Application.Models.DashBroad;

public class PatientPregnancyStatisticsResponse
{
    public string PregnancyStatus { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
