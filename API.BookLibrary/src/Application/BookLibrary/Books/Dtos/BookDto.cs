using API.AxiomManagementSystem.Domain.Common.Contracts;
using Application.BookLibrary.Authors.Dtos;
using Application.BookLibrary.Lookups.Dtos;
using Application.BookLibrary.Publishers.Dtos;
using Domain.Common.Contracts;

namespace Application.BookLibrary.Books.Dtos
{
    public class BookDto : BaseEntityDto
    {
        public string Title { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public double Price { get; set; }

        public DateTime? PublishedDate { get; set; }

        public Guid CoverTypeId { get; set; }

        public Guid? GenreId { get; set; }

        public Guid AuthorId { get; set; }

        public Guid? PublisherId { get; set; }

        public Guid? StatusId { get; set; }

        public LookupDto? Status { get; set; }

        public PublisherDto? Publisher { get; set; }

        public AuthorDto? Author { get; set; }

        public LookupDto? Genre { get; set; }

        public LookupDto? CoverType { get; set; }
    }
}
