using Application.BookLibrary.Lookups.Dtos;
using Application.BookLibrary.Lookups.Dtos.Spec;
using Application.BookLibrary.LookupTypes.Dto;
using Application.BookLibrary.LookupTypes.Spec;
using Application.Common.Exceptions;
using Application.Common.Interface;
using Application.Helpers;
using Application.Repository;
using Application.User;
using Domain.Models;

namespace Application.BookLibrary.Books.Requests
{
    public class CreateBookRequest : IRequest<Guid>
    {
        public string Title { get; set; } = string.Empty;

        public string ISBN { get; set; } = string.Empty;

        public string? Description { get; set; } = string.Empty;

        public double Price { get; set; }

        public DateTime? PublishedDate { get; set; }

        public Guid CoverTypeId { get; set; }

        public Guid? GenreId { get; set; }

        public Guid AuthorId { get; set; }

        public Guid? PublisherId { get; set; }
    }

    public class CreateBookRequestHandler : IRequestHandler<CreateBookRequest, Guid>
    {
        private readonly IRepository<Book> _repository;
        private readonly IRepository<LookupType> _lookupTypeRepository;
        private readonly IRepository<Lookup> _lookupRepository;
        private readonly IUserContext _userContext;

        public CreateBookRequestHandler(IRepository<Book> repository, IRepository<LookupType> lookupTypeRepository, IRepository<Lookup> lookupRepository, IUserContext userContext)
        {
            _repository = repository;
            _lookupTypeRepository = lookupTypeRepository;
            _lookupRepository = lookupRepository;
            _userContext = userContext;
        }

        public async Task<Guid> Handle(CreateBookRequest request, CancellationToken cancellationToken)
        {
            //TODO: we can add logger in here and change harder statuses into constants

            LookupTypeDto? lookupTypeDto = await _lookupTypeRepository.FirstOrDefaultAsync(new LookTypeByNameSpec("Book Status"));
            LookupDto? status = null;
            Guid userId = ConvertUserIdHelper.GetConvertedUserId(_userContext);
            if (lookupTypeDto != null)
            {
                List<LookupDto>? lookups = await _lookupRepository.ListAsync(new LookupByTypeIdSpec(lookupTypeDto.Id));
                if(lookups?.Count > 0)
                {
                    status = lookups.FirstOrDefault(x => x.Label.Trim().Equals("Available", StringComparison.OrdinalIgnoreCase));
                }
            }

            if (status is not null)
            {
                Book book = new Book(userId);
                book.Update(request.Title, request.ISBN, request.Description, request.Price, request.CoverTypeId, request.GenreId, request.AuthorId, request.PublisherId, null, request.PublishedDate, status.Id);
                await _repository.AddAsync(book, cancellationToken);
                return book.Id;
            }
            else
            {
                throw new NotFoundException(string.Format("StatusId was not found"));
            }
        }
    }
}
