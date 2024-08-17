using FlightsData;
using FlightsData.Models;

namespace FlightsDataTests
{
    public class DbContextTests
    {
        [Fact]
        public void Flights_can_be_saved()
        {
            // Arrange
            var flightNumber = Guid.NewGuid().ToString();
            var sut = new FlightsContext();
            var flight = GetFlight();
            flight.FlightNumber = flightNumber;
            sut.Flights.Add(flight);

            // Act
            sut.SaveChanges();

            // Assert
            var retrievedFlight = sut.Flights.FirstOrDefault(f => f.FlightNumber == flightNumber);
            Assert.NotNull(retrievedFlight);
        }

        [Fact]
        public void Saving_a_flight_assigns_an_id()
        {
            // Arrange
            var sut = new FlightsContext();
            var flight = GetFlight();
            Assert.Equal(0, flight.Id);
            sut.Flights.Add(flight);

            // Act
            sut.SaveChanges();

            // Assert
            Assert.NotEqual(0, flight.Id);
        }

        private Flight GetFlight()
        {
            return new Flight
            {
                Airline = "airline",
                ArrivalAirport = "arrival airport",
                ArrivalTime = DateTimeOffset.Now,
                DepartureAirport = "departure airport",
                DepartureTime = DateTimeOffset.Now,
                FlightNumber = "flight number",
                Status = FlightStatus.Scheduled
            };
        }
    }
}