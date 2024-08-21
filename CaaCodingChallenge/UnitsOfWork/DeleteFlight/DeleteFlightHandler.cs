using FlightsData;
using MediatR;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class DeleteFlightHandler(IDbContextFactory<FlightsContext> contextFactory)
    : IRequestHandler<DeleteFlightRequest, DeleteFlightResponse>
{
    private readonly IDbContextFactory<FlightsContext> _contextFactory = contextFactory;

    public async Task<DeleteFlightResponse> Handle(DeleteFlightRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_contextFactory);
        Guard.Against.Null(request);

        var context = await _contextFactory.CreateDbContextAsync(cancellationToken);

        var flightToDelete = await context.Flights.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        var result = new DeleteFlightResponse { Success = false };

        if (flightToDelete == null) 
        {
            return result;
        }

        context.Flights.Remove(flightToDelete);
        await context.SaveChangesAsync(cancellationToken);
        result.Success = true;

        return result;
    }
}
