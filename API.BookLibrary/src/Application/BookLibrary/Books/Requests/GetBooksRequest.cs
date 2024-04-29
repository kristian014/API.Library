using Application.BookLibrary.Books.Dtos;
using Application.BookLibrary.Books.Specs;
using Application.Repository;
using Domain.Models;

namespace Application.BookLibrary.Books.Requests
{
    public class GetBooksRequest : IRequest<List<BookDto>>
    {
    }

    public class GetBooksRequestHandler : IRequestHandler<GetBooksRequest, List<BookDto>>
    {
        private readonly IReadRepository<Book> _repository;

        public GetBooksRequestHandler(IReadRepository<Book> repository) =>
            (_repository) = (repository);

        public async Task<List<BookDto>> Handle(GetBooksRequest request, CancellationToken cancellationToken)
        {
            return await _repository.ListAsync(new GetBooksSpec());
        }
    }
}
