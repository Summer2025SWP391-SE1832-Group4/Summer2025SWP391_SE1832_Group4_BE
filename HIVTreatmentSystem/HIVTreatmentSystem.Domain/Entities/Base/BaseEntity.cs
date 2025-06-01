using System;

namespace HIVTreatmentSystem.Domain.Entities.Base
{
    public abstract class BaseEntity
    {
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }

    public abstract class BaseEntity<TKey> : BaseEntity
    {
        public TKey Id { get; set; }
    }
}
