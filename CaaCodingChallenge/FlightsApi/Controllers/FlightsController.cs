using FlightsData;
using FlightsData.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FlightsApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightsContext _flightsContext;

        public FlightsController(IFlightsContext flightsContext)
        {
            _flightsContext = flightsContext;
        }

        // GET: api/<FlightsController>
        [HttpGet]
        public IEnumerable<Flight> Get()
        {
            var x = _flightsContext.Flights.FirstOrDefault(f => f.Id == 1);
            return [GetDummyFlight()];
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
