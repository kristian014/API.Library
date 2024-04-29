using Domain.Common.Contracts;

namespace Domain.Models
{
    public class LookupType : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; } = default!;

        public LookupType()
        {
        }

        public LookupType(string name)
        {
            Name = name;
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
        }

        public LookupType Update(string? name)
        {
            if (name is not null && Name.Equals(name) is not true) Name = name;
            return this;
        }
    }
}
