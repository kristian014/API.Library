using Application.BookLibrary.Books.Requests;
using Application.Common.Validation;
using Application.Repository;
using Domain.Models;

namespace Application.BookLibrary.Books.Validators
{
    public class UpdateBookRequestValidator : CustomValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator(IReadRepository<Author> authorRepo, IReadRepository<Publisher> publisherRepo, IReadRepository<Lookup> lookupRepo)
        {
            RuleFor(p => p.Id)
              .NotEmpty()
              .NotEqual(Guid.Empty);

            RuleFor(p => p.Title)
              .NotEmpty()
              .MaximumLength(255);

            RuleFor(p => p.ISBN)
              .NotEmpty()
              .MaximumLength(255);

            RuleFor(p => p.Description)
              .NotEmpty()
              .MaximumLength(500);

            RuleFor(p => p.AuthorId)
              .NotEmpty()
              .NotEqual(Guid.Empty);


            RuleFor(p => p.CoverTypeId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

            RuleFor(p => p.PublisherId)
              .NotEmpty()
             .When(p => p.PublisherId.HasValue)
             .WithMessage((_, publisherId) => $"Publisher not found for ID {publisherId}");

            RuleFor(p => p.GenreId)
            .NotEmpty()
             .When(p => p.GenreId.HasValue)
             .WithMessage((_, genreId) => $"Genre not found for ID {genreId}");
        }
    }
}
