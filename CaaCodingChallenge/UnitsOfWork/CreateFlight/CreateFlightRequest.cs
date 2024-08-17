using FlightsData.Models;
using MediatR;

namespace UnitsOfWork;

public class CreateFlightRequest
    : IRequest<Flight?>
{
    public required Flight Flight { get; set; }
}
