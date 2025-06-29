using HIVTreatmentSystem.Domain.Entities.Base;

namespace HIVTreatmentSystem.Domain.Entities
{
    /// <summary>
    /// Entity for managing medicines/drugs in the system
    /// </summary>
    public class Medicine : BaseEntity
    {
        /// <summary>
        /// Primary key for Medicine
        /// </summary>
        public int MedicineId { get; set; }

        /// <summary>
        /// Commercial name of the medicine
        /// </summary>
        public string MedicineName { get; set; } = string.Empty;

        /// <summary>
        /// Generic/scientific name of the active ingredient
        /// </summary>
        public string? GenericName { get; set; }

        /// <summary>
        /// Manufacturer/pharmaceutical company
        /// </summary>
        public string? Manufacturer { get; set; }

        /// <summary>
        /// Dosage information (e.g., 500mg, 10ml, etc.)
        /// </summary>
        public string? Dosage { get; set; }

        /// <summary>
        /// Unit of measurement (tablet, bottle, vial, etc.)
        /// </summary>
        public string? Unit { get; set; }

        /// <summary>
        /// Description and usage instructions
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Whether this medicine is currently active/available
        /// </summary>
        public bool IsActive { get; set; } = true;

        // Navigation properties
        /// <summary>
        /// Price history for this medicine
        /// </summary>
        public virtual ICollection<MedicinePrice> MedicinePrices { get; set; } = new List<MedicinePrice>();

        /// <summary>
        /// Invoice items that include this medicine
        /// </summary>
        public virtual ICollection<InvoiceItem> InvoiceItems { get; set; } = new List<InvoiceItem>();
    }
} 