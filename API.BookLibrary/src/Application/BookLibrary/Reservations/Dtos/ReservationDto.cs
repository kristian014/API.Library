using Domain.Models.Identity;
using Application.BookLibrary.Books.Dtos;
using Application.BookLibrary.Lookups.Dtos;

namespace Application.BookLibrary.Reservations.Dtos
{
    public class ReservationDto
    {
        public Guid BookId { get; set; }

        public string UserId { get; set; } = string.Empty;

        public DateTime? ReservationDate { get; set; }

        public DateTime? ReservationCancellationDate { get; set; }

        public Guid? StatusId { get; set; }

        public LookupDto? Status { get; set; }

        public ApplicationUser? User { get; set; }

        public BookDto? Book { get; set; }
    }
}
