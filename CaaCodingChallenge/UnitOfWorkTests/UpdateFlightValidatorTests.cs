using FlightsData.Models;
using TestHelpers;
using UnitsOfWork;

namespace UnitOfWorkTests;

public class UpdateFlightValidatorTests
{
    [Fact]
    public void Id_must_not_be_non_zero()
    {
        // Arrange
        var flight = Any.Flight();
        flight.Id = 0;
        var request = new UpdateFlightRequest { Flight = flight };
        var sut = new UpdateFlightValidator();

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
    [MemberData(nameof(NullOrEmptyStrings))]
    public void FlightNumber_must_not_be_null_or_empty(string nullOrEmptyValue)
    {
        // Arrange
        var flight = Any.Flight();
        flight.FlightNumber = nullOrEmptyValue;

        AssertNullOrEmptyError(flight, nameof(Flight.FlightNumber));
    }

    [Theory]
    [MemberData(nameof(NullOrEmptyStrings))]
    public void Airline_must_not_be_null_or_empty(string nullOrEmptyValue)
    {
        // Arrange
        var flight = Any.Flight();
        flight.Airline = nullOrEmptyValue;

        AssertNullOrEmptyError(flight, nameof(Flight.Airline));
    }

    [Theory]
    [MemberData(nameof(NullOrEmptyStrings))]
    public void DepartureAirport_must_not_be_null_or_empty(string nullOrEmptyValue)
    {
        // Arrange
        var flight = Any.Flight();
        flight.DepartureAirport = nullOrEmptyValue;

        AssertNullOrEmptyError(flight, nameof(Flight.DepartureAirport));
    }

    [Theory]
    [MemberData(nameof(NullOrEmptyStrings))]
    public void ArrivalAirport_must_not_be_null_or_null(string nullOrEmptyValue)
    {
        // Arrange
        var flight = Any.Flight();
        flight.ArrivalAirport = nullOrEmptyValue;

        AssertNullOrEmptyError(flight, nameof(Flight.ArrivalAirport));
    }

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

    private void AssertNullOrEmptyError(Flight flight, string fieldName)
    {
        // Complete the arrangement
        var request = new UpdateFlightRequest { Flight = flight };
        var sut = new UpdateFlightValidator();

        // Act
        var result = sut.Validate(request);

        // Assert
        Assert.False(result.IsValid);
        var flightIdError = result
            .Errors
            .FirstOrDefault(e => e.PropertyName == $"{nameof(Flight)}.{fieldName}" && e.ErrorMessage.Contains("empty"));
        Assert.NotNull(flightIdError);
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

    public static IEnumerable<Object?[]> NullOrEmptyStrings()
    {
        yield return new[] { string.Empty };
        yield return new[] { (string?)null };
    }
}

