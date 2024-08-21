using Microsoft.EntityFrameworkCore;
using Moq;
using UnitsOfWork;
using TestHelpers;
using FlightsData;

namespace UnitOfWorkTests;

public class CreateFlightHandlerTests
{
    [Fact]
    public async Task Throws_when_request_is_null()
    {
        // Arrange
        var dbContext = new FlightsContext();
        var factory = await MockFlightsContextFactory.GetFlightsContextFactory(dbContext);
        var sut = new CreateFlightHandler(factory);
        CreateFlightRequest? request = null;

        // Assert
#pragma warning disable CS8604 // Possible null reference argument.
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.Handle(request, CancellationToken.None));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [Fact]
    public async Task Flight_is_added()
    {
        // Arrange
        var dbContext = new FlightsContext();
        var factory = await MockFlightsContextFactory.GetFlightsContextFactory(dbContext);
        var sut = new CreateFlightHandler(factory);
        var flight = Any.Flight();
        flight.Id = 0;
        var request = new CreateFlightRequest { Flight = flight };
        var numberOfFlightsBefore = await dbContext.Flights.CountAsync(CancellationToken.None);

        // Act
        await sut.Handle(request, CancellationToken.None);

        // Assert
        var numberOfFlightsAfter = await dbContext.Flights.CountAsync(CancellationToken.None);
        Assert.Equal(numberOfFlightsBefore + 1, numberOfFlightsAfter);
        
        // Tidy up
        dbContext.Flights.Remove(flight);
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

}