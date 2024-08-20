using FlightsData;
using MediatR;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class DeleteFlightHandler(IFlightsContext context)
    : IRequestHandler<DeleteFlightRequest, DeleteFlightResponse>
{
    private readonly IFlightsContext _context = context;

    public async Task<DeleteFlightResponse> Handle(DeleteFlightRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_context);
        Guard.Against.Null(_context.Flights);
        Guard.Against.Null(request);

        var flightToDelete = await _context.Flights.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
        var result = new DeleteFlightResponse { Success = false };

        if (flightToDelete == null) 
        {
            return result;
        }

        _context.Flights.Remove(flightToDelete);
        await _context.SaveChangesAsync(cancellationToken);
        result.Success = true;

        return result;
    }
}
