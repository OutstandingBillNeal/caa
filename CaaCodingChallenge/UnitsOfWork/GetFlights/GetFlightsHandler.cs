using Ardalis.GuardClauses;
using FlightsData;
using FlightsData.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class GetFlightsHandler(IDbContextFactory<FlightsContext> contextFactory)
    : IRequestHandler<GetFlightsRequest, IEnumerable<Flight>>
{
    private readonly IDbContextFactory<FlightsContext> _contextFactory = contextFactory;

    public async Task<IEnumerable<Flight>> Handle(GetFlightsRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_contextFactory);
        Guard.Against.Null(request);

        var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Flights.ToListAsync(cancellationToken);
    }
}
