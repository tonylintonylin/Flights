using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Flights.API
{
    [ApiController]
    [Menu("Bookings")]
    [Route("api/[controller]")]
    public class BookingController : Controller
    {
        private readonly IMediator _mediator;

        public BookingController(IMediator mediator) { _mediator = mediator; }
        [HttpGet]
        public async Task<ActionResult<Output>> BookingList([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }
    }
}
