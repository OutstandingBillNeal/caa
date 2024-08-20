using FlightsData.Models;
using MediatR;

namespace UnitsOfWork;

public class GetFlightByIdRequest
    : IRequest<Flight?>
{
    public int Id { get; set; }
}
