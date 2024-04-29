using Application.Common.Exceptions;
using Application.Repository;
using Domain.Models;

namespace Application.BookLibrary.Books.Requests
{
    public class DeleteBookRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public DeleteBookRequest(Guid id) => Id = id;
    }

    public class DeleteBookRequestHandler : IRequestHandler<DeleteBookRequest, Guid>
    {
        private readonly IRepository<Book> _repository;

        public DeleteBookRequestHandler(IRepository<Book> repository) =>
            (_repository) = (repository);

        public async Task<Guid> Handle(DeleteBookRequest request, CancellationToken cancellationToken)
        {

            Book? book = await _repository.GetByIdAsync(request.Id, cancellationToken);

            _ = book ?? throw new NotFoundException($"Book was not found {request.Id}");

            await _repository.DeleteAsync(book, cancellationToken);

            return request.Id;
        }
    }
}
