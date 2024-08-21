using FlightsData;
using FlightsData.Models;
using TestHelpers;

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
            var flight = Any.Flight();
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
            var flight = Any.Flight();
            flight.Id = 0;
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
            var flight = Any.Flight();
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
            var flight = Any.Flight();
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

    }
}