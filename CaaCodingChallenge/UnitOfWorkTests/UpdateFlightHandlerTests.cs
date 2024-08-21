using FlightsData;
using FlightsData.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestHelpers;
using UnitsOfWork;

namespace UnitOfWorkTests;

public class UpdateFlightHandlerTests
{
    [Fact]
    public async Task Returns_True_and_updates_flight_if_Id_exists()
    {
        // Arrange
        var dbContext = new FlightsContext();
        var factory = await MockFlightsContextFactory.GetFlightsContextFactory(dbContext);
        // Create a flight, in case none exist yet
        var flight = Any.Flight();
        flight.Id = 0;
        dbContext.Flights.Add(flight);
        await dbContext.SaveChangesAsync(CancellationToken.None);
        var newAirline = Any.String();
        Assert.NotEqual(newAirline, flight.Airline); // check we are changing it
        flight.Airline = newAirline;
        var sut = new UpdateFlightHandler(factory);
        var request = new UpdateFlightRequest { Flight = flight };
        var numberOfFlightsBefore = await dbContext.Flights.CountAsync(CancellationToken.None);

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(flight, result);
        // Check that we updated in the database
        var flightAfterUpdate = await dbContext.Flights
            .FirstOrDefaultAsync(f => f.Id == flight.Id, CancellationToken.None);
        Assert.NotNull(flightAfterUpdate);
        Assert.Equal(newAirline, flightAfterUpdate.Airline);
        // Check nothing was added or removed
        var numberOfFlightsAfter = await dbContext.Flights.CountAsync(CancellationToken.None);
        Assert.Equal(numberOfFlightsBefore, numberOfFlightsAfter);

        // Tidy up
        dbContext.Flights.Remove(result);
        await dbContext.SaveChangesAsync(CancellationToken.None);
    }

    [Fact]
    public async Task Returns_null_if_Id_does_not_exist()
    {
        // Arrange
        var dbContext = new FlightsContext();
        var factory = await MockFlightsContextFactory.GetFlightsContextFactory(dbContext);
        var sut = new UpdateFlightHandler(factory);
        var flight = Any.Flight();
        flight.Id = int.MaxValue;
        var request = new UpdateFlightRequest { Flight = flight };

        // Act
        var result = await sut.Handle(request, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}
