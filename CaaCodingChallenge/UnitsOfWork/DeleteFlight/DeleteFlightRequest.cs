using MediatR;

namespace UnitsOfWork;

public class DeleteFlightRequest
    : IRequest<DeleteFlightResponse>
{
    public int Id { get; set; }
}
