using Application.BookLibrary.Books.Requests;
using Application.Common.Validation;

namespace Application.BookLibrary.Books.Validators
{
    public class UpdateBookRequestValidator : CustomValidator<UpdateBookRequest>
    {
        public UpdateBookRequestValidator()
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
        }
    }
}
