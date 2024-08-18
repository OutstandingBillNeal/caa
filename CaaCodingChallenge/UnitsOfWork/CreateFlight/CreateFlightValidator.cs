using FluentValidation;

namespace UnitsOfWork;

public class CreateFlightValidator
    : AbstractValidator<CreateFlightRequest>
{
    public CreateFlightValidator()
    {
        RuleFor(r => r)
            .NotNull();
        RuleFor(r => r.Flight)
            .NotNull();
        RuleFor(r => r.Flight.Id)
            .Equal(0)
            .WithMessage("must be zero, if supplied");
        RuleFor(r => r.Flight.FlightNumber)
            .NotEmpty();
        RuleFor(r => r.Flight.Airline)
            .NotEmpty();
        RuleFor(r => r.Flight.DepartureAirport)
            .NotEmpty();
        RuleFor(r => r.Flight.ArrivalAirport)
            .NotEmpty();
    }
}
