using Domain.Common.Contracts;

namespace Domain.Models
{
    public class Lookup : AuditableEntity, IAggregateRoot
    {
        public string Label { get; private set; } = default!;

        public Guid TypeId { get; private set; } = default!;

        public virtual LookupType? Type { get; private set; }

        public Lookup()
        {
        }

        public Lookup(string label, Guid typeId)
        {
            Label = label;
            TypeId = typeId;
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
        }

        public Lookup(Guid id, string label, Guid typeId)
        {
            Id = id;
            Label = label;
            TypeId = typeId;
        }

        public Lookup Update(Guid? typeId, string? label)
        {
            if (typeId is not null && TypeId.Equals(typeId) is not true) TypeId = (Guid)typeId;
            if (label is not null && Label.Equals(label) is not true) Label = label;
            return this;
        }
    }
}
