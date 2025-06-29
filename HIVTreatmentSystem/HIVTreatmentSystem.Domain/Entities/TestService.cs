using HIVTreatmentSystem.Domain.Entities.Base;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Entity for managing test services offered by the medical facility
    /// </summary>
    public class TestService : BaseEntity
    {
        /// <summary>
        /// Primary key for TestService
        /// </summary>
        public int TestServiceId { get; set; }

        /// <summary>
        /// Name of the test service
        /// </summary>
        public string ServiceName { get; set; } = string.Empty;

        /// <summary>
        /// Unique code for the service (for internal tracking)
        /// </summary>
        public string? ServiceCode { get; set; }

        /// <summary>
        /// Category of test (HIV, CD4, Viral Load, Blood Test, etc.)
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Description of the test service
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Estimated duration for the test in minutes
        /// </summary>
        public int? EstimatedDuration { get; set; }

        /// <summary>
        /// Whether this test service is currently active/available
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation properties
        /// <summary>
        /// Price history for this test service
        /// </summary>
        public virtual ICollection<TestServicePrice> TestServicePrices { get; set; } = new List<TestServicePrice>();

        /// <summary>
        /// Invoice items that include this test service
        /// </summary>
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
} 