using FlightsData;
using Moq;
using UnitsOfWork;

namespace UnitOfWorkTests;

public class GetFlightsHandlerTests
{
    [Fact]
    public async Task Throws_when_request_is_null()
    {
        // Arrange
        var dbContextMock = new Mock<IFlightsContext>();
        var sut = new GetFlightsHandler(dbContextMock.Object);
        GetFlightsRequest? request = null;

        // Assert
#pragma warning disable CS8604 // Possible null reference argument.
        await Assert.ThrowsAsync<ArgumentNullException>(async () => await sut.Handle(request, CancellationToken.None));
#pragma warning restore CS8604 // Possible null reference argument.
    }

    [Fact]
    public async Task Returns_all_Flights()
    {
        // Arrange
        var dbContext = new FlightsContext();
        var sut = new GetFlightsHandler(dbContext);
        var request = new GetFlightsRequest();
        var numberOfFlightsInDb = dbContext.Flights.Count();

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(numberOfFlightsInDb, result.Count());
    }

}
