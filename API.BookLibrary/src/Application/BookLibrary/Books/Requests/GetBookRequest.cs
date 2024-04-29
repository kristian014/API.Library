using Application.BookLibrary.Books.Dtos;
using Application.BookLibrary.Books.Specs;
using Application.Common.Exceptions;
using Application.Repository;
using Domain.Models;

namespace Application.BookLibrary.Books.Requests
{
    public class GetBookRequest : IRequest<BookDto>
    {
        public Guid Id { get; set; }

        public GetBookRequest(Guid id) => Id = id;
    }

    public class GetBookRequestHandler : IRequestHandler<GetBookRequest, BookDto>
    {
        private readonly IRepository<Book> _repository;

        public GetBookRequestHandler(IRepository<Book> repository) =>
            (_repository) = (repository);

        public async Task<BookDto> Handle(GetBookRequest request, CancellationToken cancellationToken)
        {
            BookDto? bookDto = await _repository.FirstOrDefaultAsync(new GetBookByIdSpec(request.Id), cancellationToken);
            _ = bookDto ?? throw new NotFoundException(string.Format("Book not found for Id", request.Id));

            return bookDto;
        }
    }
}
