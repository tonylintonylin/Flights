using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using static Flights.API.List;

namespace Flights.API
{
    [ApiController]
    [Menu("Bookings")]
    [Route("api/[controller]")]
    public class BookingsController : Controller
    {
        private readonly IMediator _mediator;

        public BookingsController(IMediator mediator) { _mediator = mediator; }
        [HttpGet]
        public async Task<ActionResult<Output>> BookingList([FromQuery]List.Query query)
        {
            return await _mediator.Send(query);
        }
    }
}
