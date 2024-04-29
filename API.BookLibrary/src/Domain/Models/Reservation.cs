using Domain.Common.Contracts;
using Domain.Models.Identity;

namespace Domain.Models
{
    public class Reservation : AuditableEntity, IAggregateRoot
    {
        public Guid BookId { get; private set; }

        public string UserId { get; private set; } = string.Empty;

        public DateTime? ReservationDate { get; private set; }

        public DateTime? ReservationCancellationDate { get; private set; }

        public Guid? StatusId { get; private set; }

        public virtual Lookup? Status { get; private set; }

        public virtual ApplicationUser? User { get; private set; }

        public virtual Book? Book { get; private set; }

        public Reservation(Guid? createdBy)
        {
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
        }


        public Reservation Update(Guid? bookId, DateTime? reservationDate, DateTime? reservationCancellationDate, string? userId, Guid? lastModifiedBy, Guid? statusId)
        {
            bool isUpdated = false;

            if (bookId != null && bookId != Guid.Empty && !BookId.Equals(bookId)) { BookId = (Guid)bookId; isUpdated = true; }
            if (statusId != null && statusId != Guid.Empty && !StatusId.Equals(statusId)) { StatusId = (Guid)statusId; isUpdated = true; }
            if (reservationDate.HasValue && (ReservationDate != reservationDate.Value)) { ReservationDate = reservationDate.Value; isUpdated = true; }
            if (reservationCancellationDate.HasValue && (ReservationCancellationDate != reservationCancellationDate.Value)) { ReservationCancellationDate = reservationCancellationDate.Value; isUpdated = true; }
            if (userId != null &&!string.IsNullOrEmpty(userId) && !UserId.Equals(userId)) { UserId = userId; isUpdated = true; }

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
