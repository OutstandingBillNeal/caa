using FlightsData;
using TestHelpers;
using UnitsOfWork;

namespace UnitOfWorkTests;

public class DeleteFlightHandlerTests
{
    [Fact]
    public async Task Returns_True_and_deletes_flight_if_Id_exists()
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
        var sut = new DeleteFlightHandler(factory);
        var request = new DeleteFlightRequest { Id = flight.Id };
        var numberOfFlightsBefore = dbContext.Flights.Count();

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
        var flightAfterDelete = dbContext.Flights.FirstOrDefault(f => f.Id == flight.Id);
        Assert.Null(flightAfterDelete);
        var numberOfFlightsAfter = dbContext.Flights.Count();
        Assert.Equal(numberOfFlightsBefore - 1, numberOfFlightsAfter);
    }

    [Fact]
    public async Task Returns_false_and_does_not_delete_anything_if_Id_does_not_exist()
    {
        // Arrange
        var dbContext = new FlightsContext();
        var factory = await MockFlightsContextFactory.GetFlightsContextFactory(dbContext);
        var sut = new DeleteFlightHandler(factory);
        var request = new DeleteFlightRequest { Id = int.MaxValue };
        var numberOfFlightsBefore = dbContext.Flights.Count();

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.Success);
        var numberOfFlightsAfter = dbContext.Flights.Count();
        Assert.Equal(numberOfFlightsBefore, numberOfFlightsAfter);
    }
}
