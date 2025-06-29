using HIVTreatmentSystem.Domain.Entities.Base;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Entity for managing invoices/bills in the medical billing system
    /// </summary>
    public class Invoice : BaseEntity
    {
        /// <summary>
        /// Primary key for Invoice
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Unique invoice number (auto-generated)
        /// </summary>
        public string InvoiceNumber { get; set; } = string.Empty;

        /// <summary>
        /// Foreign key to Patient
        /// </summary>
        public int PatientId { get; set; }

        /// <summary>
        /// Foreign key to Doctor
        /// </summary>
        public int DoctorId { get; set; }

        /// <summary>
        /// Foreign key to Appointment (nullable - some invoices may not be tied to appointments)
        /// </summary>
        public int? AppointmentId { get; set; }

        /// <summary>
        /// Date the invoice was issued
        /// </summary>
        public DateTime InvoiceDate { get; set; }

        /// <summary>
        /// Due date for payment
        /// </summary>
        public DateTime DueDate { get; set; }

        /// <summary>
        /// Subtotal amount before taxes and discounts
        /// </summary>
        public decimal SubTotal { get; set; }

        /// <summary>
        /// Tax amount (VAT, etc.)
        /// </summary>
        public decimal TaxAmount { get; set; }

        /// <summary>
        /// Discount amount applied
        /// </summary>
        public decimal DiscountAmount { get; set; }

        /// <summary>
        /// Final total amount after taxes and discounts
        /// </summary>
        public decimal TotalAmount { get; set; }

        /// <summary>
        /// Current status of the invoice
        /// </summary>
        public InvoiceStatus InvoiceStatus { get; set; } = InvoiceStatus.Draft;

        /// <summary>
        /// Additional notes for the invoice
        /// </summary>
        public string? Notes { get; set; }

        /// <summary>
        /// User ID who created this invoice
        /// </summary>
        public int? CreatedBy { get; set; }

        // Navigation properties
        /// <summary>
        /// The patient this invoice belongs to
        /// </summary>
        public virtual Patient Patient { get; set; } = null!;

        /// <summary>
        /// The doctor who provided the services
        /// </summary>
        public virtual Doctor Doctor { get; set; } = null!;

        /// <summary>
        /// The appointment this invoice is related to (if any)
        /// </summary>
        public virtual Appointment? Appointment { get; set; }

        /// <summary>
        /// Items/services included in this invoice
        /// </summary>
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();

        /// <summary>
        /// Payments made against this invoice
        /// </summary>
        public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
    }
} 