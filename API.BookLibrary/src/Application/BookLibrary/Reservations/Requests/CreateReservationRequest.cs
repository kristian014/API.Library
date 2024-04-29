using Application.BookLibrary.Lookups.Dtos.Spec;
using Application.BookLibrary.Lookups.Dtos;
using Application.BookLibrary.LookupTypes.Dto;
using Application.BookLibrary.LookupTypes.Spec;
using Application.Common.Exceptions;
using Application.Helpers;
using Application.Repository;
using Domain.Models;
using Application.Common.Interface;
using Microsoft.Extensions.Logging;

namespace Application.BookLibrary.Reservations.Requests
{
    public class CreateReservationRequest : IRequest<Guid>
    {
        public Guid BookId { get; private set; }
    }

    public class CreateReservationRequestHandler : IRequestHandler<CreateReservationRequest, Guid>
    {
        private readonly IRepository<Reservation> _repository;
        private readonly IRepository<LookupType> _lookupTypeRepository;
        private readonly IRepository<Lookup> _lookupRepository;
        private readonly IUserContext _userContext;
        private readonly ILogger<CreateReservationRequestHandler> _logger;

        public CreateReservationRequestHandler(IRepository<Reservation> repository, IRepository<LookupType> lookupTypeRepository, IRepository<Lookup> lookupRepository, IUserContext userContext, ILogger<CreateReservationRequestHandler> logger)
        {
           _lookupRepository = lookupRepository;
            _lookupTypeRepository = lookupTypeRepository;
            _userContext = userContext;
            _repository = repository;
            _logger = logger;
        }
        public async Task<Guid> Handle(CreateReservationRequest request, CancellationToken cancellationToken)
        {
            // here is an example where I have used a logger to log errors
            try
            {
                LookupTypeDto? lookupTypeDto = await _lookupTypeRepository.FirstOrDefaultAsync(new LookTypeByNameSpec("Reservation Status"));
                LookupDto? status = null;
                Guid userId = ConvertUserIdHelper.GetConvertedUserId(_userContext);

                if (lookupTypeDto != null)
                {
                    List<LookupDto>? lookups = await _lookupRepository.ListAsync(new LookupByTypeIdSpec(lookupTypeDto.Id));
                    if (lookups?.Count > 0)
                    {
                        status = lookups.FirstOrDefault(x => x.Label.Trim().Equals("Pending", StringComparison.OrdinalIgnoreCase));
                    }
                }

                if (status is not null)
                {
                    // if a book is reserved and nothing is done, the system should auto cancel that reservation.
                    // Here we are setting a reservation cancellation days to 3
                    Reservation reservation = new Reservation(userId);
                    reservation.Update(request.BookId, DateTime.Now, DateTime.Now.AddDays(3), userId.ToString(), userId, status.Id);
                    await _repository.AddAsync(reservation, cancellationToken);
                    return reservation.Id;
                }
                else
                {
                    throw new NotFoundException(string.Format("StatusId was not found"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to save reservation {ex.Message}");
                throw;
            }
           
        }
    }
}
