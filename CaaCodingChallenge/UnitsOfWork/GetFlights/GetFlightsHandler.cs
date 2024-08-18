using Ardalis.GuardClauses;
using FlightsData;
using FlightsData.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class GetFlightsHandler(IFlightsContext context)
    : IRequestHandler<GetFlightsRequest, IEnumerable<Flight>>
{
    private readonly IFlightsContext _context = context;

    public async Task<IEnumerable<Flight>> Handle(GetFlightsRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_context);
        Guard.Against.Null(_context.Flights);

        return await _context.Flights.ToListAsync(cancellationToken);
    }
}
