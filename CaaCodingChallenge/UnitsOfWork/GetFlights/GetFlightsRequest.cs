using FlightsData.Models;
using MediatR;

namespace UnitsOfWork.GetFlights
{
    public class GetFlightsRequest
        : IRequest<IEnumerable<Flight>>
    { }
}
