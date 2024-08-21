using FluentValidation;

namespace UnitsOfWork;

public class CreateFlightValidator
    : AbstractValidator<CreateFlightRequest>
{
    public CreateFlightValidator()
    {
        RuleFor(r => r.Flight.Id)
            .Equal(0)
            .WithMessage("must be zero, if supplied");
        RuleFor(r => r.Flight.Status)
            .IsInEnum();
    }
}
