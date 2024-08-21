using FlightsApi.Controllers;
using FlightsData.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using TestHelpers;
using UnitsOfWork;

namespace FlightsApiTests;

public class FlightsControllerTests
{
    private readonly Mock<IMediator> _mediator = new();
    private readonly Mock<IValidator<CreateFlightRequest>> _createFlightValidator = new();
    private readonly Mock<IValidator<UpdateFlightRequest>> _updateFlightValidator = new();

    [Fact]
    public async Task Get_returns_values_with_200()
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

        // Assert general cleanliness
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ActionResult<IEnumerable<Flight>>>(result);
        Assert.NotNull(result.Result);
        // Assert 200 - Ok
        Assert.IsType<OkObjectResult>(result.Result);
        var okObjectResult = (OkObjectResult)result.Result;
        Assert.NotNull(okObjectResult);
        Assert.NotNull(okObjectResult.Value);
        Assert.IsAssignableFrom<IEnumerable<Flight>>(okObjectResult.Value);
        var returnedFlights = (IEnumerable<Flight>)okObjectResult.Value;
        var returnedFlight1 = returnedFlights.FirstOrDefault(f => f.FlightNumber == flight1.FlightNumber);
        var returnedFlight2 = returnedFlights.FirstOrDefault(f => f.FlightNumber == flight2.FlightNumber);
        Assert.NotNull(returnedFlight1);
        Assert.NotNull(returnedFlight2);
        // Assert correct data returned
        Assert.Equal(flight1, returnedFlight1);
        Assert.Equal(flight2, returnedFlight2);
    }

    [Fact]
    public async Task Get_by_id_returns_value_with_200_when_found()
    {
        // Arrange
        var flight1 = Any.Flight();
        _mediator
            .Setup(m => m.Send(It.IsAny<GetFlightByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(flight1);

        // Act
        var result = await Sut.GetById(Any.Int());

        // Assert general cleanliness
        Assert.NotNull(result);
        Assert.IsAssignableFrom<ActionResult<Flight>>(result);
        Assert.NotNull(result.Result);
        // Assert 200 - Ok
        Assert.IsType<OkObjectResult>(result.Result);
        var okObjectResult = (OkObjectResult)result.Result;
        Assert.NotNull(okObjectResult);
        Assert.NotNull(okObjectResult.Value);
        Assert.IsAssignableFrom<Flight>(okObjectResult.Value);
        var returnedFlight = (Flight)okObjectResult.Value;
        Assert.NotNull(returnedFlight);
        // Assert correct data returned
        Assert.Equal(flight1, returnedFlight);
    }

    [Fact]
    public async Task Get_by_id_returns_value_with_404_when_not_found()
    {
        // Arrange
        _mediator
            .Setup(m => m.Send(It.IsAny<GetFlightByIdRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Flight?)null);

        // Act
        var result = await Sut.GetById(Any.Int());

        // Assert general cleanliness
        Assert.NotNull(result);
        Assert.NotNull(result.Result);
        // Assert 404 - Not found
        Assert.IsAssignableFrom<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Post_valid_flight_returns_value_and_location_with_201()
    {
        // Arrange
        var flight1 = Any.Flight();
        _mediator
            .Setup(m => m.Send(It.IsAny<CreateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(flight1);
        var validResult = new ValidationResult();
        Assert.True(validResult.IsValid); // Show the reader the implied validity.
        _createFlightValidator
            .Setup(m => m.ValidateAsync(It.IsAny<CreateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validResult);

        // Act
        var result = await Sut.Post(flight1);

        // Some people debate the "correctness" of asserting multiple things in a single test.
        // The argument is along the lines of "One test - one reason for failure.  A test
        // failure can point directly to the problem without further investigation."
        // While that argument has some merit, if one assumes that the test will pass more
        // often than it will fail, then the relative importance of legibility increases.
        // A less verbose, less repetetive test class is more legible than a more verbose one.
        // The downside of multiple assertions in a test is that it requires some effort to
        // determine which assertion failed.  However, this is usually quite a simple task.

        // Assert general cleanliness
        Assert.NotNull(result);
        // Assert 201 - Created
        Assert.IsType<CreatedAtActionResult>(result);
        var createdResult = (CreatedAtActionResult)result;
        Assert.NotNull(createdResult);
        Assert.NotNull(createdResult.Value);
        Assert.IsAssignableFrom<Flight>(createdResult.Value);
        var returnedFlight = (Flight)createdResult.Value;
        Assert.NotNull(returnedFlight);
        // Assert correct data returned
        Assert.Equal(flight1, returnedFlight);
        // Assert correct location returned
        Assert.Equal(nameof(FlightsController.GetById), createdResult.ActionName);
        Assert.NotNull(createdResult.RouteValues);
        // ... and by the time you get down here, where the value to be tested requires several
        // steps of derivation, it becomes less practcal to put each assertion into its own test.
        Assert.Equal(returnedFlight.Id, createdResult.RouteValues["id"]);
    }

    [Fact]
    public async Task Post_invalid_flight_returns_400()
    {
        // Arrange
        var flight1 = Any.Flight();
        _mediator
            .Setup(m => m.Send(It.IsAny<CreateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(flight1);
        var invalidResult = new ValidationResult([new ValidationFailure()]);
        Assert.False(invalidResult.IsValid); // Show the reader the implied validity.
        _createFlightValidator
            .Setup(m => m.ValidateAsync(It.IsAny<CreateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(invalidResult);

        // Act
        var result = await Sut.Post(flight1);

        // Assert general cleanliness
        Assert.NotNull(result);
        // Assert 400 - Bad request
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Put_valid_flight_returns_value_and_location_with_200()
    {
        // Arrange
        var flight1 = Any.Flight();
        _mediator
            .Setup(m => m.Send(It.IsAny<UpdateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(flight1);
        var validResult = new ValidationResult();
        Assert.True(validResult.IsValid); // Show the reader the implied validity.
        _updateFlightValidator
            .Setup(m => m.ValidateAsync(It.IsAny<UpdateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validResult);

        // Act
        var result = await Sut.Put(flight1.Id, flight1);

        // Assert general cleanliness
        Assert.NotNull(result);
        // Assert 200 - Ok
        Assert.IsType<OkObjectResult>(result);
        var okResult = (OkObjectResult)result;
        Assert.NotNull(okResult);
        Assert.NotNull(okResult.Value);
        Assert.IsAssignableFrom<Flight>(okResult.Value);
        var returnedFlight = (Flight)okResult.Value;
        Assert.NotNull(returnedFlight);
        // Assert correct data returned
        Assert.Equal(flight1, returnedFlight);
    }

    [Fact]
    public async Task Put_invalid_flight_returns_400()
    {
        // Arrange
        var flight1 = Any.Flight();
        _mediator
            .Setup(m => m.Send(It.IsAny<UpdateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(flight1);
        var invalidResult = new ValidationResult([new ValidationFailure()]);
        Assert.False(invalidResult.IsValid); // Show the reader the implied validity.
        _updateFlightValidator
            .Setup(m => m.ValidateAsync(It.IsAny<UpdateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(invalidResult);

        // Act
        var result = await Sut.Put(flight1.Id, flight1);

        // Assert general cleanliness
        Assert.NotNull(result);
        // Assert 400 - Bad request
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task Put_non_existent_flight_returns_404()
    {
        // Arrange
        Flight? nullFlight = null;
        _mediator
            .Setup(m => m.Send(It.IsAny<UpdateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(nullFlight);
        var validResult = new ValidationResult();
        Assert.True(validResult.IsValid); // Show the reader the implied validity.
        _updateFlightValidator
            .Setup(m => m.ValidateAsync(It.IsAny<UpdateFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(validResult);

        // Act
        var result = await Sut.Put(Any.Int(), Any.Flight());

        // Assert general cleanliness
        Assert.NotNull(result);
        // Assert 404 - Not found
        Assert.IsType<NotFoundResult>(result);
    }

    [Fact]
    public async Task Delete_valid_flight_returns_204()
    {
        // Arrange
        var deleteResponse = new DeleteFlightResponse { Success = true };
        _mediator
            .Setup(m => m.Send(It.IsAny<DeleteFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(deleteResponse);

        // Act
        var result = await Sut.Delete(Any.Int());

        // Assert general cleanliness
        Assert.NotNull(result);
        // Assert 204 - No content
        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Delete_non_existent_flight_returns_404()
    {
        // Arrange
        var deleteResponse = new DeleteFlightResponse { Success = false };
        _mediator
            .Setup(m => m.Send(It.IsAny<DeleteFlightRequest>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(deleteResponse);

        // Act
        var result = await Sut.Delete(Any.Int());

        // Assert general cleanliness
        Assert.NotNull(result);
        // Assert 404 - Not found
        Assert.IsType<NotFoundResult>(result);
    }

    private FlightsController Sut
        => new(_mediator.Object, _createFlightValidator.Object, _updateFlightValidator.Object);

}
