
namespace HIVTreatmentSystem.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendEmailAsync(string to, string subject, string body, bool isHtml = true);
        Task SendEmailAsync(string[] to, string subject, string body, bool isHtml = true);
        Task SendAppointmentReminderAsync(string to, string patientName, string doctorName, DateTime appointmentTime, string location);
        Task SendMedicationReminderAsync(string to, string patientName, string medicationName, string dosage, string instructions);
        Task SendPasswordResetAsync(string to, string resetLink, string userName);
        Task SendAccountVerificationAsync(string to, string verificationLink, string userName);
        Task SendTestResultsNotificationAsync(string to, string patientName, string testType, DateTime testDate);
    }
}
