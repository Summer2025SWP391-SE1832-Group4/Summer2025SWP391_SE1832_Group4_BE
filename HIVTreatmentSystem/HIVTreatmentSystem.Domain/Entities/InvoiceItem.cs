using HIVTreatmentSystem.Domain.Entities.Base;
using HIVTreatmentSystem.Domain.Enums;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Entity for managing individual items within an invoice
    /// </summary>
    public class InvoiceItem : BaseEntity
    {
        /// <summary>
        /// Primary key for InvoiceItem
        /// </summary>
        public int InvoiceItemId { get; set; }

        /// <summary>
        /// Foreign key to Invoice
        /// </summary>
        public int InvoiceId { get; set; }

        /// <summary>
        /// Type of item (Medicine, TestService, Consultation, etc.)
        /// </summary>
        public InvoiceItemType ItemType { get; set; }

        /// <summary>
        /// ID of the specific item (MedicineId, TestServiceId, etc.)
        /// </summary>
        public int? ItemId { get; set; }

        /// <summary>
        /// Name/description of the item
        /// </summary>
        public string ItemName { get; set; } = string.Empty;

        /// <summary>
        /// Quantity of the item
        /// </summary>
        public decimal Quantity { get; set; }

        /// <summary>
        /// Unit price of the item
        /// </summary>
        public decimal UnitPrice { get; set; }

        /// <summary>
        /// Total price for this line item (Quantity × UnitPrice)
        /// </summary>
        public decimal TotalPrice { get; set; }

        /// <summary>
        /// Foreign key to TestResult (if this item is related to a specific test)
        /// </summary>
        public int? TestResultId { get; set; }

        // Navigation properties
        /// <summary>
        /// The invoice this item belongs to
        /// </summary>
        public virtual Invoice Invoice { get; set; } = null!;

        /// <summary>
        /// The medicine (if ItemType is Medicine)
        /// </summary>
        public virtual Medicine? Medicine { get; set; }

        /// <summary>
        /// The test service (if ItemType is TestService)
        /// </summary>
        public virtual TestService? TestService { get; set; }

        /// <summary>
        /// The test result this item is related to (if applicable)
        /// </summary>
        public virtual TestResult? TestResult { get; set; }
    }
} 