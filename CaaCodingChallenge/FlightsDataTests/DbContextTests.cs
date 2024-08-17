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

        [Fact]
        public void Flights_can_be_deleted()
        {
            // Arrange
            var sut = new FlightsContext();
            var flight = GetFlight();
            sut.Flights.Add(flight);
            sut.SaveChanges();
            var flightId = flight.Id;

            // Act
            sut.Flights.Remove(flight);
            sut.SaveChanges();

            // Assert
            var flightRetrievedAfterDeletion = sut.Flights.FirstOrDefault(f => f.Id == flightId);
            Assert.Null(flightRetrievedAfterDeletion);
        }

        [Fact]
        public void Flights_can_be_updated()
        {
            // Arrange
            var sut = new FlightsContext();
            var flight = GetFlight();
            var initialFlightNumber = Guid.NewGuid().ToString();
            flight.FlightNumber = initialFlightNumber;
            sut.Flights.Add(flight);
            sut.SaveChanges();
            var flightId = flight.Id;

            // Act
            var updatedFlightNumber = Guid.NewGuid().ToString();
            flight.FlightNumber = updatedFlightNumber;
            sut.SaveChanges();

            // Assert
            var flightRetrievedAfterUpdate = sut.Flights.First(f => f.Id == flightId);
            Assert.Equal(updatedFlightNumber, flightRetrievedAfterUpdate.FlightNumber);
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