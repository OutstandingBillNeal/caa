using FluentValidation;

namespace UnitsOfWork;

public class UpdateFlightValidator
    : AbstractValidator<UpdateFlightRequest>
{
    public UpdateFlightValidator()
    {
        RuleFor(r => r.Flight.Id)
            .NotEqual(0)
            .WithMessage("must be non-zero");
        RuleFor(r => r.Flight.FlightNumber)
            .NotEmpty();
        RuleFor(r => r.Flight.Airline)
            .NotEmpty();
        RuleFor(r => r.Flight.DepartureAirport)
            .NotEmpty();
        RuleFor(r => r.Flight.ArrivalAirport)
            .NotEmpty();
        RuleFor(r => r.Flight.Status)
            .IsInEnum();
    }
}
