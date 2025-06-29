namespace HIVTreatmentSystem.Domain.Enums
{
    /// <summary>
    /// Enum for Invoice Item Types
    /// </summary>
    public enum InvoiceItemType
    {
        /// <summary>
        /// Medicine/drug item
        /// </summary>
        Medicine = 1,

        /// <summary>
        /// Test service item
        /// </summary>
        TestService = 2,

        /// <summary>
        /// Consultation/appointment fee
        /// </summary>
        Consultation = 3,

        /// <summary>
        /// Other miscellaneous services
        /// </summary>
        Other = 4
    }
} 