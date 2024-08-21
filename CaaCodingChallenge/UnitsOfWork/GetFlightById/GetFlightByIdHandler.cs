using FlightsData;
using FlightsData.Models;
using MediatR;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class GetFlightByIdHandler(IDbContextFactory<FlightsContext> contextFactory)
    : IRequestHandler<GetFlightByIdRequest, Flight?>
{
    private readonly IDbContextFactory<FlightsContext> _contextFactory = contextFactory;

    public async Task<Flight?> Handle(GetFlightByIdRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_contextFactory);
        Guard.Against.Null(request);

        var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        return await context.Flights.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
    }
}
