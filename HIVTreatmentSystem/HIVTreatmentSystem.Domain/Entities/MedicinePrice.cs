using HIVTreatmentSystem.Domain.Entities.Base;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Entity for managing medicine pricing with time-based pricing support
    /// </summary>
    public class MedicinePrice : BaseEntity
    {
        /// <summary>
        /// Primary key for MedicinePrice
        /// </summary>
        public int MedicinePriceId { get; set; }

        /// <summary>
        /// Foreign key to Medicine
        /// </summary>
        public int MedicineId { get; set; }

        /// <summary>
        /// Selling price to patients
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Cost price for the hospital/clinic
        /// </summary>
        public decimal? CostPrice { get; set; }

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
        /// The medicine this price applies to
        /// </summary>
        public virtual Medicine Medicine { get; set; } = null!;
    }
} 