using Domain.Common.Contracts;

namespace Domain.Models
{
    public class Book : AuditableEntity, IAggregateRoot
    {
        public string Title { get; private set; } = string.Empty;

        public string ISBN { get; private set; } = string.Empty;

        public string? Description { get; private set; } = string.Empty;

        public double Price { get; private set; }

        public DateTime? PublishedDate { get; private set; }

        public Guid CoverTypeId { get; private set; }

        public Guid? GenreId { get; private set; }

        public Guid AuthorId { get; private set; }

        public Guid? PublisherId { get; private set; }

        public Guid StatusId { get; private set; }

        public virtual Lookup? Status { get; private set; }

        public virtual Publisher? Publisher { get; private set; }

        public virtual Author? Author { get; private set; }

        public virtual Lookup? Genre { get; private set; }

        public virtual Lookup? CoverType { get; private set; }

        public Book(Guid? createdBy = null)
        {
            CreatedOn = DateTime.UtcNow;
            LastModifiedOn = DateTime.UtcNow;
            CreatedBy = createdBy;
        }

        public Book Update(string? title, string? isbn, string? description, double? price,
                       Guid? coverTypeId, Guid? genreId, Guid? authorId, Guid? publisherId, Guid? lastModifiedBy, DateTime? publishedDate, Guid? statusId)
        {
            bool isUpdated = false;

            if (title != null && !Title.Equals(title)) { Title = title; isUpdated = true; }
            if (isbn != null && !ISBN.Equals(isbn)) { ISBN = isbn; isUpdated = true; }
            if (description != null && !string.IsNullOrEmpty(description) && !Description?.Equals(description) is not true) { Description = description; isUpdated = true; }
            if (price.HasValue && Price != price.Value) { Price = price.Value; isUpdated = true; }
            if (statusId != null && statusId != Guid.Empty && !StatusId.Equals(statusId)) { StatusId = (Guid)statusId; isUpdated = true; }
            if (coverTypeId.HasValue && CoverTypeId != coverTypeId.Value) { CoverTypeId = coverTypeId.Value; isUpdated = true; }
            if (genreId.HasValue && GenreId != genreId.Value) { GenreId = genreId.Value; isUpdated = true; }
            if (authorId.HasValue && AuthorId != authorId.Value) { AuthorId = authorId.Value; isUpdated = true; }
            if (publisherId.HasValue && PublisherId != publisherId) { PublisherId = publisherId; isUpdated = true; }
            if (publishedDate is not null && PublishedDate != publishedDate) { PublishedDate = publishedDate.Value; isUpdated = true; }

            if (isUpdated && lastModifiedBy.HasValue && lastModifiedBy.Value != Guid.Empty)
            {
                LastModifiedBy = lastModifiedBy.Value;
                LastModifiedOn = DateTime.UtcNow;
            }

            return this;
        }

    }
}
