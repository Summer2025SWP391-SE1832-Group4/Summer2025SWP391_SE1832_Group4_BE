namespace HIVTreatmentSystem.Application.Models.DashBroad;

public class PatientGenderStatisticsResponse
{
    public string Gender { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
