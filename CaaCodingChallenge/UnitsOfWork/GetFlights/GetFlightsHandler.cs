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
        return await _context.Flights.ToListAsync(cancellationToken);
    }
}
