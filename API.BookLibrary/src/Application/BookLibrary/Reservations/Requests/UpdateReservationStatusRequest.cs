using Application.Common.Exceptions;
using Application.Common.Interface;
using Application.Helpers;
using Application.Repository;
using Domain.Models;
using Microsoft.Extensions.Logging;

namespace Application.BookLibrary.Reservations.Requests
{
    // TODO Admin or staff can now update the status of the reservation to 
    public class UpdateReservationStatusRequest : IRequest<Guid>
    {
        public Guid Id { get; set; }

        public Guid StatusId { get; set; }
    }

    public class UpdateReservationStatusRequestHandler : IRequestHandler<UpdateReservationStatusRequest, Guid>
    {
        private readonly IRepository<Reservation> _repository;
        private readonly IUserContext _userContext;
        private readonly ILogger<CreateReservationRequestHandler> _logger;

        public UpdateReservationStatusRequestHandler(IRepository<Reservation> repository, IUserContext userContext, ILogger<CreateReservationRequestHandler> logger)
        {
            _userContext = userContext;
            _repository = repository;
            _logger = logger;
        }
        public async Task<Guid> Handle(UpdateReservationStatusRequest request, CancellationToken cancellationToken)
        {
            try
            {
                Guid userId = ConvertUserIdHelper.GetConvertedUserId(_userContext);

                Reservation? reservation = await _repository.GetByIdAsync(request.Id, cancellationToken);

                _ = reservation ?? throw new NotFoundException($"Reservation was not found {request.Id}");
                reservation.Update(null, null, null, userId.ToString(), userId, request.StatusId);
                await _repository.AddAsync(reservation, cancellationToken);
                return reservation.Id;


            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occured while trying to save reservation {ex.Message}");
                throw;
            }

        }
    }
}