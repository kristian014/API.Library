using Application.BookLibrary.Lookups.Dtos.Spec;
using Application.BookLibrary.Lookups.Dtos;
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
    public class UpdateReservationRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }
        public Guid BookId { get; set; }

        public DateTime ReservationDate { get; set; }
    }

    public class UpdateReservationRequestHandler : IRequestHandler<UpdateReservationRequest, Guid>
    {
        private readonly IRepository<Reservation> _repository;
        private readonly IUserContext _userContext;
        private readonly ILogger<CreateReservationRequestHandler> _logger;
        private readonly IRepository<LookupType> _lookupTypeRepository;
        private readonly IRepository<Lookup> _lookupRepository;

        public UpdateReservationRequestHandler(IRepository<Reservation> repository, IUserContext userContext, ILogger<CreateReservationRequestHandler> logger, IRepository<LookupType> lookupTypeRepository, IRepository<Lookup> lookupRepository)
        {
            _userContext = userContext;
            _repository = repository;
            _logger = logger;
            _lookupTypeRepository = lookupTypeRepository;
            _lookupRepository = lookupRepository;
        }
        public async Task<Guid> Handle(UpdateReservationRequest request, CancellationToken cancellationToken)
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
                    Reservation? reservation = await _repository.GetByIdAsync(request.Id, cancellationToken);

                    _ = reservation ?? throw new NotFoundException($"Reservation was not found {request.Id}");
                    reservation.Update(request.BookId, request.ReservationDate, null, userId.ToString(), userId, status.Id);
                    await _repository.UpdateAsync(reservation, cancellationToken);
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
