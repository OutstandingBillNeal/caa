using FlightsApi.Controllers;
using FlightsData.Models;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestHelpers;
using UnitsOfWork;

namespace FlightsApiTests;

public class FlightsApiTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IValidator<CreateFlightRequest>> createFlightValidator = new();
    private readonly Mock<IValidator<UpdateFlightRequest>> updateFlightValidator = new();

    [Fact]
    public async Task GetFlights_returns_values_as_JsonResult_with_200()
    {
        // Arrange
        var flight1 = Any.Flight();
        var flight2 = Any.Flight();
        var mediatorResult = new[] { flight1, flight2 };
        _mediator
            .Setup(m => m.Send(It.IsAny<GetFlightsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        // Act
        var result = await Sut.Get();

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        Assert.IsType<JsonResult>(result.Result);
        var jsonResult = (JsonResult)result.Result;
        Assert.NotNull(jsonResult);
        Assert.NotNull(jsonResult.Value);
        Assert.IsAssignableFrom<IEnumerable<Flight>>(jsonResult.Value);
        var returnedFlights = (IEnumerable<Flight>)jsonResult.Value;
        var returnedFlight1 = returnedFlights.FirstOrDefault(f => f.FlightNumber == flight1.FlightNumber);
        var returnedFlight2 = returnedFlights.FirstOrDefault(f => f.FlightNumber == flight2.FlightNumber);
        Assert.NotNull(returnedFlight1);
        Assert.NotNull(returnedFlight2);
        Assert.Equal(flight1, returnedFlight1);
        Assert.Equal(flight2, returnedFlight2);
    }

    private FlightsController Sut
        => new(_mediator.Object, createFlightValidator.Object, updateFlightValidator.Object);

}
