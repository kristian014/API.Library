using Application.BookLibrary.Reservations.Requests;
using Application.Common.Validation;
using Application.Repository;
using Domain.Models;

namespace Application.BookLibrary.Reservations.Validators
{
    public class CreateReservationRequestValidator : CustomValidator<CreateReservationRequest>
    {
        public CreateReservationRequestValidator(IReadRepository<Book> bookRepo)
        {
            RuleFor(p => p.BookId)
            .NotEmpty()
            .NotEqual(Guid.Empty);

            RuleFor(p => p.ReservationDate)
            .NotEmpty();
        }
    }
}
