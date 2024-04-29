using Application.BookLibrary.Books.Dtos;
using Ardalis.Specification;
using Domain.Models;

namespace Application.BookLibrary.Books.Specs
{
    public class GetBookByIdSpec : Specification<Book, BookDto>
    {
        public GetBookByIdSpec(Guid id)
        {
            Query
            .Where(p => p.Id == id)
            .Include(x => x.Genre)
            .Include(x => x.Status)
            .Include(x => x.Author)
            .Include(x => x.Publisher);
        }
    }
}
