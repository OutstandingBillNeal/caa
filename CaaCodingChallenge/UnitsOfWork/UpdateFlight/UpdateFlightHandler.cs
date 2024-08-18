﻿using Ardalis.GuardClauses;
using FlightsData;
using FlightsData.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace UnitsOfWork;

public class UpdateFlightHandler(IFlightsContext context)
    : IRequestHandler<UpdateFlightRequest, Flight?>
{
    private readonly IFlightsContext _context = context;

    public async Task<Flight?> Handle(UpdateFlightRequest request, CancellationToken cancellationToken)
    {
        Guard.Against.Null(_context);
        Guard.Against.Null(_context.Flights);
        Guard.Against.Null(request);
        Guard.Against.Null(request.Flight);

        var retrievedFlight = await _context
            .Flights
            .FirstOrDefaultAsync(f => f.Id == request.Flight.Id, cancellationToken);

        if (retrievedFlight == null)
        {
            return retrievedFlight;
        }

        retrievedFlight.FlightNumber = request.Flight.FlightNumber;
        retrievedFlight.Airline = request.Flight.Airline;
        retrievedFlight.DepartureAirport = request.Flight.DepartureAirport;
        retrievedFlight.ArrivalAirport = request.Flight.ArrivalAirport;
        retrievedFlight.DepartureTime = request.Flight.DepartureTime;
        retrievedFlight.ArrivalTime = request.Flight.ArrivalTime;
        retrievedFlight.Status = request.Flight.Status;

        await _context.SaveChangesAsync(cancellationToken);
        var updatedFlight = await _context
            .Flights
            .FirstOrDefaultAsync(f => f.Id == request.Flight.Id, cancellationToken);

        return updatedFlight;
    }
}
