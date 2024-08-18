using FlightsData;
using FlightsData.Models;
using MediatR;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class GetFlightByIdHandler(IFlightsContext context)
    : IRequestHandler<GetFlightByIdRequest, Flight>
{
    private readonly IFlightsContext _context = context;

    public async Task<Flight> Handle(GetFlightByIdRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_context);
        Guard.Against.Null(_context.Flights);
        Guard.Against.Null(request);

#pragma warning disable CS8603 // Possible null reference return. Ardalis checked this.
        return await _context.Flights.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
#pragma warning restore CS8603 // Possible null reference return.
    }
}
