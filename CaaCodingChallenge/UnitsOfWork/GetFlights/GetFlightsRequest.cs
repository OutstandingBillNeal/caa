using FlightsData.Models;
using MediatR;

namespace UnitsOfWork;

public class GetFlightsRequest
    : IRequest<IEnumerable<Flight>>
{ }
