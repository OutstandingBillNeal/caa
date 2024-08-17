using FlightsData.Models;
using MediatR;

namespace UnitsOfWork.CreateFlight
{
    public class CreateFlightRequest
        : IRequest<Flight?>
    {
        public required Flight Flight { get; set; }
    }
}
