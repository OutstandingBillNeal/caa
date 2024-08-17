using FlightsData.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitsOfWork;

namespace FlightsApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController(IMediator mediator)
        : ControllerBase
    {
        private readonly IMediator _mediator = mediator;

        // GET: api/<FlightsController>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Flight>>> Get()
        {
            var request = new GetFlightsRequest();
            var result = await _mediator.Send(request, CancellationToken.None);
            return new JsonResult(result);
        }

        // GET api/<FlightsController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Flight>> Get(int id)
        {
            var request = new GetFlightByIdRequest { Id = id };
            var result = await _mediator.Send(request, CancellationToken.None);

            return result == null
                ? new NotFoundResult()
                : new JsonResult(result);
        }

        // POST api/<FlightsController>
        [HttpPost]
        public async Task<ActionResult<Flight>> Post([FromBody] Flight value)
        {
            var request = new CreateFlightRequest { Flight = value };
            var result = await _mediator.Send(request, CancellationToken.None);
            return new JsonResult(result);
        }

        // PUT api/<FlightsController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Flight>> Put(int id, [FromBody] string value)
        {
            return GetDummyFlight();
        }

        // DELETE api/<FlightsController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            return new JsonResult(false);
        }

        private Flight GetDummyFlight()
        {
            return new Flight { Id = 1, Airline = "a", ArrivalAirport = "b", ArrivalTime = DateTimeOffset.Now, DepartureAirport = "c", DepartureTime = DateTimeOffset.Now, FlightNumber = "d", Status = FlightStatus.Scheduled };
        }
    }
}
