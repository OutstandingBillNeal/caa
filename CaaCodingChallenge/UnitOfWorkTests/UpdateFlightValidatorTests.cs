using FlightsData.Models;
using TestHelpers;
using UnitsOfWork;

namespace UnitOfWorkTests;

public class UpdateFlightValidatorTests
{
    [Theory]
    [InlineData(-1)]
    [InlineData(5)]
    [InlineData(17)]
    public void Status_must_be_valid(int invalidValue)
    {
        // Arrange
        var flight = Any.Flight();
        flight.Status = (FlightStatus)invalidValue;
        var request = new UpdateFlightRequest { Flight = flight };
        var sut = new UpdateFlightValidator();

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
        flight.Status = (FlightStatus)statusValue;
        var request = new UpdateFlightRequest { Flight = flight };
        var sut = new UpdateFlightValidator();

        // Act
        var result = sut.Validate(request);

        // Assert
        Assert.True(result.IsValid);
    }

}

