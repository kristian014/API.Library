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
        }
    }
}
