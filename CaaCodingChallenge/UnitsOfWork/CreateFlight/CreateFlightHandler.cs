using Ardalis.GuardClauses;
using FlightsData;
using FlightsData.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class CreateFlightHandler
    : IRequestHandler<CreateFlightRequest, Flight?>
{
    private readonly IFlightsContext _context;

    public CreateFlightHandler(IFlightsContext context)
    {
        _context = context;
    }
    public async Task<Flight?> Handle(CreateFlightRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(request);
        //Guard.Against.Null(nameof(request.Flight));

        _context.Flights.Add(request.Flight);
        await _context.SaveChangesAsync(cancellationToken);
        var createdFlight = await _context.Flights.FirstOrDefaultAsync(f => f.Id == request.Flight.Id);
        return createdFlight;
    }
}
