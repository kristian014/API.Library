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
              .NotEqual(Guid.Empty)
               .MustAsync(async (id, ct) => await authorRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => string.Format("author.notfound", id));

            RuleFor(p => p.CoverTypeId)
            .NotEmpty()
            .NotEqual(Guid.Empty)
             .MustAsync(async (id, ct) => await lookupRepo.GetByIdAsync(id, ct) is not null)
                .WithMessage((_, id) => string.Format("covertype.notfound", id));

            RuleFor(p => p.PublisherId)
             .MustAsync(async (publisherId, cancellationToken) =>
                 publisherId.HasValue && await publisherRepo.GetByIdAsync(publisherId.Value, cancellationToken) != null)
             .When(p => p.PublisherId.HasValue)  // This ensures the rule is checked only if PublisherId is not null
             .WithMessage((_, publisherId) => $"Publisher not found for ID {publisherId}");

            RuleFor(p => p.GenreId)
             .MustAsync(async (genreId, cancellationToken) =>
                 genreId.HasValue && await lookupRepo.GetByIdAsync(genreId.Value, cancellationToken) != null)
             .When(p => p.GenreId.HasValue)
             .WithMessage((_, genreId) => $"Genre not found for ID {genreId}");
        }
    }
}
