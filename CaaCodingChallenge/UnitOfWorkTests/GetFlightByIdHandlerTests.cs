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
        // Create a flight, in case none exist yet
        var flight = Any.Flight();
        flight.Id = 0;
        dbContext.Flights.Add(flight);
        dbContext.SaveChanges();
        // flight.Id will now have a non-zero value
        var sut = new GetFlightByIdHandler(dbContext);
        var request = new GetFlightByIdRequest { Id = flight.Id };

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(flight.Airline, result.Airline);

        // Tidy up
        dbContext.Flights.Remove(result);
        dbContext.SaveChanges();
    }

    [Fact]
    public async Task Returns_null_if_Id_does_not_exist()
    {
        // Arrange
        var dbContext = new FlightsContext();
        var sut = new GetFlightByIdHandler(dbContext);
        var request = new GetFlightByIdRequest { Id = int.MaxValue };

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
