using Domain.Common.Contracts;

namespace Domain.Models
{
    public class Author : AuditableEntity, IAggregateRoot
    {
        public string Name { get; private set; } = string.Empty;
        public DateTime? DateOfBirth { get; private set; }
        public ICollection<Book>? Books { get; private set; }

        public Author(string name, DateTime? dateOfBirth, Guid? createdBy = null)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        public Author Update(string? name, DateTime? dateOfBirth, Guid lastModifiedBy)
        {
            bool isUpdated = false;

            if (name != null && !Name.Equals(name)) { Name = name; isUpdated = true; }
            if (dateOfBirth.HasValue && (DateOfBirth != dateOfBirth.Value)) { DateOfBirth = dateOfBirth; isUpdated = true; }

            // Only update the last modified details if a change has occurred
            if (isUpdated)
            {
                LastModifiedBy = lastModifiedBy;
                LastModifiedOn = DateTime.UtcNow;
            }

            return this;
        }
    }
}
