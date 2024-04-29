using Application.Common.Exceptions;
using Application.Common.Interface;
using Application.Helpers;
using Application.Repository;
using Application.User;
using Domain.Models;

namespace Application.BookLibrary.Books.Requests
{
    public class UpdateBookRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public double Price { get; set; }

        public DateTime? PublishedDate { get; set; }

        public Guid CoverTypeId { get; set; }

        public Guid? GenreId { get; set; }

        public Guid AuthorId { get; set; }

        public Guid? PublisherId { get; set; }

        public Guid? StatusId { get; set; }
    }

    public class UpdateBookRequestHandler : IRequestHandler<UpdateBookRequest, Guid>
    {
        private readonly IRepository<Book> _repository;
        private readonly IUserContext _userContext;

        public UpdateBookRequestHandler(IRepository<Book> repository, IUserContext userContext)
        {
            _repository = repository;
            _userContext = userContext;
        }

        public async Task<Guid> Handle(UpdateBookRequest request, CancellationToken cancellationToken)
        {
            Guid userId = ConvertUserIdHelper.GetConvertedUserId(_userContext);

            Book? book = await _repository.GetByIdAsync(request.Id, cancellationToken);

            _ = book ?? throw new NotFoundException($"Book was not found {request.Id}");

            book.Update(request.Title, request.ISBN, request.Description, request.Price, request.CoverTypeId, request.GenreId, request.AuthorId, request.PublisherId, userId, request.PublishedDate, request.StatusId);
            await _repository.UpdateAsync(book, cancellationToken);

            return book.Id;
        }
    }
}
