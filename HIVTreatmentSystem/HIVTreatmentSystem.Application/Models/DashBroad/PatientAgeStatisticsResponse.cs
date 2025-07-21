namespace HIVTreatmentSystem.Application.Models.DashBroad;

public class PatientAgeStatisticsResponse
{
    public string AgeRange { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
