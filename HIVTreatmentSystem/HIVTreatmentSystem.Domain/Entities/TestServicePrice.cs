using HIVTreatmentSystem.Domain.Entities.Base;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Entity for managing test service pricing with time-based pricing support
    /// </summary>
    public class TestServicePrice : BaseEntity
    {
        /// <summary>
        /// Primary key for TestServicePrice
        /// </summary>
        public int TestServicePriceId { get; set; }

        /// <summary>
        /// Foreign key to TestService
        /// </summary>
        public int TestServiceId { get; set; }

        /// <summary>
        /// Price for the test service
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Date when this price becomes effective
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Date when this price expires (nullable for current prices)
        /// </summary>
        public DateTime? ExpiryDate { get; set; }

        /// <summary>
        /// Whether this price is currently active
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation properties
        /// <summary>
        /// The test service this price applies to
        /// </summary>
        public virtual TestService TestService { get; set; } = null!;
    }
} 