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
        var numberOfFlightsBefore = dbContext.Flights.Count();

        // Act
        await sut.Handle(request, CancellationToken.None);

        // Assert
        var numberOfFlightsAfter = dbContext.Flights.Count();
        Assert.Equal(numberOfFlightsBefore + 1, numberOfFlightsAfter);
        
        // Tidy up
        dbContext.Flights.Remove(flight);
        dbContext.SaveChanges();
    }

    private static DbSet<T> GetEmptyQueryableMockDbSet<T>() where T : class
    {
        var sourceList = new List<T>();
        return GetQueryableMockDbSet(sourceList);
    }

    private static DbSet<T> GetQueryableMockDbSet<T>(List<T> sourceList) where T : class
    {
        var queryable = sourceList.AsQueryable();

        var dbSet = new Mock<DbSet<T>>();
        dbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
        dbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
        dbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
        dbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(() => queryable.GetEnumerator());
        dbSet.Setup(d => d.Add(It.IsAny<T>())).Callback<T>((s) => sourceList.Add(s));

        return dbSet.Object;
    }
}