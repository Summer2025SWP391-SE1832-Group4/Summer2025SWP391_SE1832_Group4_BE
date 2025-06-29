namespace HIVTreatmentSystem.Domain.Enums
{
    /// <summary>
    /// Enum for Invoice Status
    /// </summary>
    public enum InvoiceStatus
    {
        /// <summary>
        /// Invoice is in draft state, not yet issued
        /// </summary>
        Draft = 0,

        /// <summary>
        /// Invoice has been issued to patient
        /// </summary>
        Issued = 1,

        /// <summary>
        /// Invoice has been fully paid
        /// </summary>
        Paid = 2,

        /// <summary>
        /// Invoice has been partially paid
        /// </summary>
        PartiallyPaid = 3,

        /// <summary>
        /// Invoice has been cancelled
        /// </summary>
        Cancelled = 4,

        /// <summary>
        /// Invoice is overdue (past due date)
        /// </summary>
        Overdue = 5
    }
} 