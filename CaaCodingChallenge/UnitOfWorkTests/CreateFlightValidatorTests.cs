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

        [Theory]
        [InlineData(-1)]
        [InlineData(5)]
        [InlineData(9)]
        public void Status_must_be_valid(int invalidValue)
        {
            // Arrange
            var flight = Any.Flight();
            flight.Id = 0;
            flight.Status = (FlightStatus)invalidValue;
            var request = new CreateFlightRequest { Flight = flight };
            var sut = new CreateFlightValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.False(result.IsValid);
            var flightStatusError = result
                .Errors
                .FirstOrDefault(e => e.PropertyName == $"{nameof(Flight)}.{nameof(Flight.Status)}");
            Assert.NotNull(flightStatusError);
        }

        [Theory]
        [InlineData((int)FlightStatus.Scheduled)]
        [InlineData((int)FlightStatus.Delayed)]
        [InlineData((int)FlightStatus.Cancelled)]
        [InlineData((int)FlightStatus.InAir)]
        [InlineData((int)FlightStatus.Landed)]
        public void Valid_flights_pass_validation(int statusValue)
        {
            // Arrange
            var flight = Any.Flight();
            flight.Id = 0;
            flight.Status = (FlightStatus)statusValue;
            var request = new CreateFlightRequest { Flight = flight };
            var sut = new CreateFlightValidator();

            // Act
            var result = sut.Validate(request);

            // Assert
            Assert.True(result.IsValid);
        }

    }
}
