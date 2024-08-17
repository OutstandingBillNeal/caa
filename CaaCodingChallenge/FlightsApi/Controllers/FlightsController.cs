using FlightsData;
using FlightsData.Models;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitsOfWork.GetFlights;

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
        public async Task<IEnumerable<Flight>> Get()
        {
            var request = new GetFlightsRequest();
            var result = await _mediator.Send(request, CancellationToken.None);
            return result;
        }

        // GET api/<FlightsController>/5
        [HttpGet("{id}")]
        public Flight Get(int id)
        {
            return GetDummyFlight();
        }

        // POST api/<FlightsController>
        [HttpPost]
        public Flight Post([FromBody] string value)
        {
            return GetDummyFlight();
        }

        // PUT api/<FlightsController>/5
        [HttpPut("{id}")]
        public Flight Put(int id, [FromBody] string value)
        {
            return GetDummyFlight();
        }

        // DELETE api/<FlightsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        private Flight GetDummyFlight()
        {
            return new Flight { Id = 1, Airline = "a", ArrivalAirport = "b", ArrivalTime = DateTimeOffset.Now, DepartureAirport = "c", DepartureTime = DateTimeOffset.Now, FlightNumber = "d", Status = FlightStatus.Scheduled };
        }
    }
}
