using Domain.Common.Contracts;
using System.Net;

namespace Domain.Models
{
    public class Rental : AuditableEntity, IAggregateRoot
    {
        public string UserId { get; private set; } = string.Empty;

        public DateTime RentalDate { get; private set; }

        public DateTime DueDate { get; private set; }

        public DateTime? ReturnDate { get; private set; }

        public ICollection<Book>? Books { get; private set; }

        public Rental(Guid? createdBy = null)
        {
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
        }

        public Rental Update(DateTime? rentalDate, DateTime? dueDate, string? userId, Guid? lastModifiedBy, DateTime? returnDate)
        {
            bool isUpdated = false;

            if (rentalDate.HasValue && (RentalDate != rentalDate.Value)) { RentalDate = rentalDate.Value; isUpdated = true; }
            if (dueDate.HasValue && (DueDate != dueDate.Value)) { DueDate = dueDate.Value; isUpdated = true; }
            if (returnDate.HasValue && (ReturnDate != returnDate.Value)) { ReturnDate = returnDate.Value; isUpdated = true; }
            if (userId != null && !string.IsNullOrEmpty(userId) && !UserId.Equals(userId)) { UserId = userId; isUpdated = true; }

            // Only update the last modified details if a change has occurred
            if (isUpdated && lastModifiedBy.HasValue)
            {
                LastModifiedBy = lastModifiedBy.Value;
                LastModifiedOn = DateTime.UtcNow;
            }

            return this;
        }
    }
}
