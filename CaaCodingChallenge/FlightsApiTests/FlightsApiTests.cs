using FlightsApi.Controllers;
using FluentValidation;
using MediatR;
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
    public async Task GetFlights_sends_mediator_request()
    {
        // Arrange
        var mediatorResult = new[] { Any.Flight(), Any.Flight() };
        _mediator
            .Setup(m => m.Send(It.IsAny<GetFlightsRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(mediatorResult);

        // Act
        var result = await Sut.Get();

        // Assert
        Assert.Equal(new JsonResult(mediatorResult), result);
    }

    private FlightsController Sut
        => new FlightsController(_mediator.Object, createFlightValidator.Object, updateFlightValidator.Object);
}
