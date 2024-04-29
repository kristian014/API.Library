using Application.BookLibrary.Books.Requests;
using Application.Common.Validation;

namespace Application.BookLibrary.Books.Validators
{
    public class CreateBookRequestValidator : CustomValidator<CreateBookRequest>
    {
        public CreateBookRequestValidator()
        {
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
