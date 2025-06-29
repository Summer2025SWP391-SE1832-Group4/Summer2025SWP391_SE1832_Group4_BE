namespace HIVTreatmentSystem.Domain.Enums
{
    /// <summary>
    /// Enum for Payment Status
    /// </summary>
    public enum PaymentStatus
    {
        /// <summary>
        /// Payment is pending processing
        /// </summary>
        Pending = 1,

        /// <summary>
        /// Payment has been completed successfully
        /// </summary>
        Completed = 2,

        /// <summary>
        /// Payment has failed
        /// </summary>
        Failed = 3,

        /// <summary>
        /// Payment has been cancelled
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// Payment has been refunded
        /// </summary>
        Refunded = 5
    }
} 