using Application.BookLibrary.Books.Dtos;
using Ardalis.Specification;
using Domain.Models;

namespace Application.BookLibrary.Books.Specs
{
    public class GetBooksSpec : Specification<Book, BookDto>
    {
        public GetBooksSpec() =>
           Query
            .OrderBy(x => x.CreatedOn)
            .Include(x => x.Genre)
            .Include(x => x.Status)
            .Include(x => x.Author)
            .Include(x => x.Publisher);
    }
}
