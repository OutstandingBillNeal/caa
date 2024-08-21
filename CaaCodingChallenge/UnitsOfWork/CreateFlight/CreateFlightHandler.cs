using Ardalis.GuardClauses;
using FlightsData;
using FlightsData.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class CreateFlightHandler(IDbContextFactory<FlightsContext> contextFactory)
    : IRequestHandler<CreateFlightRequest, Flight?>
{
    private readonly IDbContextFactory<FlightsContext> _contextFactory = contextFactory;

    public async Task<Flight?> Handle(CreateFlightRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_contextFactory);
        Guard.Against.Null(request);
        Guard.Against.Null(request.Flight);

        var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        context.Flights.Add(request.Flight);
        await context.SaveChangesAsync(cancellationToken);

        return await context.Flights.FirstOrDefaultAsync(f => f.Id == request.Flight.Id, cancellationToken);

    }
}
