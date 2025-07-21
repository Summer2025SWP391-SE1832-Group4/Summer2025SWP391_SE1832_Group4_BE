namespace HIVTreatmentSystem.Application.Models.DashBroad;

public class PatientTreatmentStatusStatisticsResponse
{
    public string TreatmentStatus { get; set; } = string.Empty;
    public int Count { get; set; }
    public double Percentage { get; set; }
}
