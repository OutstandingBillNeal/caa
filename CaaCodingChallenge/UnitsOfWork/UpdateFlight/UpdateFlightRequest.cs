using FlightsData.Models;
using MediatR;

namespace UnitsOfWork;

public class UpdateFlightRequest
    : IRequest<Flight?>
{
    public required Flight Flight { get; set; }
}
