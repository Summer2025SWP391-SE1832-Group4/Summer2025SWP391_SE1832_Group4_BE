

namespace HIVTreatmentSystem.Domain.Enums
{
    public enum AppointmentStatus
    {
        Scheduled,
        Completed,
        CancelledByPatient,
        CancelledByDoctor,
        CheckedIn,
        NoShow,
        PendingConfirmation,
        InProgress,
    }
}
