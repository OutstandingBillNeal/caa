using FlightsData;
using FlightsData.Models;
using MediatR;
using Ardalis.GuardClauses;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class GetFlightByIdHandler(IFlightsContext context)
    : IRequestHandler<GetFlightByIdRequest, Flight?>
{
    private readonly IFlightsContext _context = context;

    public async Task<Flight?> Handle(GetFlightByIdRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_context);
        Guard.Against.Null(_context.Flights);
        Guard.Against.Null(request);

        return await _context.Flights.FirstOrDefaultAsync(f => f.Id == request.Id, cancellationToken);
    }
}
