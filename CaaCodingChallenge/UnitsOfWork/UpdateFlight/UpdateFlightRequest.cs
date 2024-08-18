using FlightsData.Models;
using MediatR;

namespace UnitsOfWork;

public class UpdateFlightRequest
    : IRequest<Flight?>
{
    public Flight Flight { get; set; }
}
