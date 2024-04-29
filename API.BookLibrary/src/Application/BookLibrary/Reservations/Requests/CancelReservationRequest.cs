using Application.BookLibrary.Lookups.Dtos;
using Application.BookLibrary.Lookups.Dtos.Spec;
using Application.BookLibrary.LookupTypes.Dto;
using Application.BookLibrary.LookupTypes.Spec;
using Application.Common.Exceptions;
using Application.Common.Interface;
using Application.Helpers;
using Application.Repository;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.BookLibrary.Reservations.Requests
{
    public class CancelReservationRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
    }

    public class CancelReservationRequestHandler : IRequestHandler<CancelReservationRequest, Guid>
    {
        private readonly IRepository<Reservation> _repository;
        private readonly ILogger<CancelReservationRequestHandler> _logger;
        private readonly IUserContext _userContext;
        private readonly IRepository<LookupType> _lookupTypeRepository;
        private readonly IRepository<Lookup> _lookupRepository;
        public CancelReservationRequestHandler(IRepository<Reservation> repository, ILogger<CancelReservationRequestHandler> logger, IUserContext userContext, IRepository<LookupType> lookupTypeRepository, IRepository<Lookup> lookupRepository)
        {
            _repository = repository;
            _logger = logger;
            _userContext = userContext;
            _lookupTypeRepository = lookupTypeRepository;
            _lookupRepository = lookupRepository;
        }
        public async Task<Guid> Handle(CancelReservationRequest request, CancellationToken cancellationToken)
        {
            // here is an example where I have used a logger to log errors
            try
            {
                LookupTypeDto? lookupTypeDto = await _lookupTypeRepository.FirstOrDefaultAsync(new LookTypeByNameSpec("Reservation Status"));
                LookupDto? status = null;
                if (lookupTypeDto is not null)
                {
                    List<LookupDto>? lookups = await _lookupRepository.ListAsync(new LookupByTypeIdSpec(lookupTypeDto.Id));
                    if (lookups?.Count > 0)
                    {
                        status = lookups.FirstOrDefault(x => x.Label.Trim().Equals("Cancelled", StringComparison.OrdinalIgnoreCase));
                    }
                }

                if (status is not null)
                {
                    Reservation? reservation = await _repository.GetByIdAsync(request.Id, cancellationToken);
                    Guid userId = ConvertUserIdHelper.GetConvertedUserId(_userContext);

                    _ = reservation ?? throw new NotFoundException($"Reservation was not found {request.Id}");

                    reservation.Update(DateTime.Now, userId, status.Id);
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
