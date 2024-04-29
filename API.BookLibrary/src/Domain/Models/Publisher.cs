using Domain.Common.Contracts;

namespace Domain.Models
{
    public class Publisher : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; } = string.Empty;

        public ICollection<Book>? Books { get; private set; }

        // Constructor for creating a new Publisher
        public Publisher(string name, Guid? createdBy)
        {
            Name = name;

            // Set initial audit information
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        // Update method for updating existing Publisher
        public Publisher Update(string? name, Guid lastModifiedBy)
        {
            bool isUpdated = false;

            if (name is not null && !Name.Equals(name)) { Name = name; isUpdated = true; }

            if (isUpdated) { LastModifiedBy = lastModifiedBy; LastModifiedOn = DateTime.UtcNow; }

            return this;
        }
    }

}
