using HIVTreatmentSystem.Domain.Entities.Base;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Entity for managing payments against invoices
    /// </summary>
    public class Payment : BaseEntity
    {
        /// <summary>
        /// Primary key for Payment
        /// </summary>
        public int PaymentId { get; set; }

        /// <summary>
        /// Foreign key to Invoice
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Date the payment was made
        /// </summary>
        public DateTime PaymentDate { get; set; }

        /// <summary>
        /// Method of payment (Cash, Card, Transfer, etc.)
        /// </summary>
        public PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// Amount paid
        /// </summary>
        public decimal PaymentAmount { get; set; }

        /// <summary>
        /// Reference number for the payment (transaction ID, check number, etc.)
        /// </summary>
        public string? PaymentReference { get; set; }

        /// <summary>
        /// Status of the payment
        /// </summary>
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        /// <summary>
        /// Additional notes about the payment
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// User ID who processed this payment
        /// </summary>
        public int? CreatedBy { get; set; }

        // Navigation properties
        /// <summary>
        /// The invoice this payment is for
        /// </summary>
        public virtual Invoice Invoice { get; set; } = null!;
    }
} 