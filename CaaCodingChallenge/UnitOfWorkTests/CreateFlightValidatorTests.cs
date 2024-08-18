using FlightsData.Models;
using TestHelpers;
using UnitsOfWork;

namespace UnitOfWorkTests
{
    public class CreateFlightValidatorTests
    {
        [Fact]
        public void Id_must_not_be_non_zero()
        {
            // Arrange
            var flight = Any.Flight();
            Assert.NotEqual(0, flight.Id);
            var request = new CreateFlightRequest { Flight = flight };
            var sut = new CreateFlightValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            var flightIdError = result
                .Errors
                .FirstOrDefault(e => e.PropertyName == $"{nameof(Flight)}.{nameof(Flight.Id)}" && e.ErrorMessage.Contains("zero"));
            Assert.NotNull(flightIdError);
        }

        [Fact]
        public void FlightNumber_must_not_be_empty()
        {
            // Arrange
            var flight = Any.Flight();
            flight.FlightNumber = string.Empty;

            AssertNullOrEmptyError(flight, nameof(Flight.FlightNumber));
        }

        [Fact]
        public void FlightNumber_must_not_be_null()
        {
            // Arrange
            var flight = Any.Flight();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.  Actually, you can, apparently.
            flight.FlightNumber = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            AssertNullOrEmptyError(flight, nameof(Flight.FlightNumber));
        }

        [Fact]
        public void Airline_must_not_be_empty()
        {
            // Arrange
            var flight = Any.Flight();
            flight.Airline = string.Empty;

            AssertNullOrEmptyError(flight, nameof(Flight.Airline));
        }

        [Fact]
        public void Airline_must_not_be_null()
        {
            // Arrange
            var flight = Any.Flight();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.  Actually, you can, apparently.
            flight.Airline = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            AssertNullOrEmptyError(flight, nameof(Flight.Airline));
        }

        [Fact]
        public void DepartureAirport_must_not_be_empty()
        {
            // Arrange
            var flight = Any.Flight();
            flight.DepartureAirport = string.Empty;

            AssertNullOrEmptyError(flight, nameof(Flight.DepartureAirport));
        }

        [Fact]
        public void DepartureAirport_must_not_be_null()
        {
            // Arrange
            var flight = Any.Flight();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.  Actually, you can, apparently.
            flight.DepartureAirport = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            AssertNullOrEmptyError(flight, nameof(Flight.DepartureAirport));
        }

        [Fact]
        public void ArrivalAirport_must_not_be_empty()
        {
            // Arrange
            var flight = Any.Flight();
            flight.ArrivalAirport = string.Empty;

            AssertNullOrEmptyError(flight, nameof(Flight.ArrivalAirport));
        }

        [Fact]
        public void ArrivalAirport_must_not_be_null()
        {
            // Arrange
            var flight = Any.Flight();
#pragma warning disable CS8625 // Cannot convert null literal to non-nullable reference type.  Actually, you can, apparently.
            flight.ArrivalAirport = null;
#pragma warning restore CS8625 // Cannot convert null literal to non-nullable reference type.

            AssertNullOrEmptyError(flight, nameof(Flight.ArrivalAirport));
        }

        private void AssertNullOrEmptyError(Flight flight, string fieldName)
        {
            // Complete the arrangement
            var request = new CreateFlightRequest { Flight = flight };
            var sut = new CreateFlightValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            var flightIdError = result
                .Errors
                .FirstOrDefault(e => e.PropertyName == $"{nameof(Flight)}.{fieldName}" && e.ErrorMessage.Contains("empty"));
            Assert.NotNull(flightIdError);
        }

    }
}
