using Application.BookLibrary.Books.Dtos;
using Application.BookLibrary.Books.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Host.Controllers
{
    [Authorize]
    public class BooksController : BaseApiController
    {
        [HttpGet("getall")]
        public async Task<List<BookDto>> GetAllAsync()
        {
            return await Mediator.Send(new GetBooksRequest());
        }

        [HttpGet("{id:guid}")]
        public async Task<BookDto> GetAsync(Guid id)
        {
            return await Mediator.Send(new GetBookRequest(id));
        }

        [HttpPost("create")]
        public async Task<Guid> CreateAsync(CreateBookRequest request)
        {
            return await Mediator.Send(request);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateAsync(UpdateBookRequest request, Guid id)
        {
            return id != request.Id
                ? BadRequest()
                : Ok(await Mediator.Send(request));
        }

        [HttpDelete("{id:guid}")]
        public async Task<Guid> DeleteAsync(Guid id)
        {
            return await Mediator.Send(new DeleteBookRequest(id));
        }

    }
}
