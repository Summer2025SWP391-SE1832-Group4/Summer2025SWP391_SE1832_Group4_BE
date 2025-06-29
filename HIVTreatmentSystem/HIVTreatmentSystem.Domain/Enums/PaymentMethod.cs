namespace HIVTreatmentSystem.Domain.Enums
{
    /// <summary>
    /// Enum for Payment Methods
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// Cash payment
        /// </summary>
        Cash = 1,

        /// <summary>
        /// Credit/Debit card payment
        /// </summary>
        Card = 2,

        /// <summary>
        /// Bank transfer
        /// </summary>
        BankTransfer = 3,

        /// <summary>
        /// Insurance payment
        /// </summary>
        Insurance = 4,

        /// <summary>
        /// Online payment (e-wallet, etc.)
        /// </summary>
        Online = 5,

        /// <summary>
        /// Check payment
        /// </summary>
        Check = 6
    }
} 