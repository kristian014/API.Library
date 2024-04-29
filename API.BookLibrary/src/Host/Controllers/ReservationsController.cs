using Application.BookLibrary.Reservations.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize]
    public class ReservationsController : BaseApiController
    {
        [HttpPost("cancel-reservation")]
        public async Task<Guid> CancelReservationAsync(CancelReservationRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPost("create")]
        public async Task<Guid> CreateAsync(CreateReservationRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("update")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateReservationRequest request, Guid id)
        {
            return id != request.Id
                ? BadRequest()
                : Ok(await Mediator.Send(request));
        }

        [HttpPost("update-reservation-status")]
        public async Task<ActionResult<Guid>> UpdateReservationStatusAsync(UpdateReservationRequest request, Guid id)
        {
            return id != request.Id
                ? BadRequest()
                : Ok(await Mediator.Send(request));
        }
    }
}
