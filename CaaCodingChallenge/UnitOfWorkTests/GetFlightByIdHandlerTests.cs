using FlightsData;
using TestHelpers;
using UnitsOfWork;

namespace UnitOfWorkTests;

public class GetFlightByIdHandlerTests
{
    [Fact]
    public async Task Returns_flight_if_Id_exists()
    {
        // Arrange
        var dbContext = new FlightsContext();
        var factory = await MockFlightsContextFactory.GetFlightsContextFactory(dbContext);
        // Create a flight, in case none exist yet
        var flight = Any.Flight();
        flight.Id = 0;
        dbContext.Flights.Add(flight);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        // flight.Id will now have a non-zero value
        var sut = new GetFlightByIdHandler(factory);
        var request = new GetFlightByIdRequest { Id = flight.Id };

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(flight.Airline, result.Airline);

        // Tidy up
        var flightToRemove = dbContext.Flights.First(f => f.Id == flight.Id);
        dbContext.Flights.Remove(flightToRemove);
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Returns_null_if_Id_does_not_exist()
    {
        // Arrange
        var dbContext = new FlightsContext();
        var factory = await MockFlightsContextFactory.GetFlightsContextFactory(dbContext);
        var sut = new GetFlightByIdHandler(factory);
        var request = new GetFlightByIdRequest { Id = int.MaxValue };

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
