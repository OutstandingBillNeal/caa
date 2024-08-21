using FluentValidation;

namespace UnitsOfWork;

public class UpdateFlightValidator
    : AbstractValidator<UpdateFlightRequest>
{
    public UpdateFlightValidator()
    {
        RuleFor(r => r.Flight.Status)
            .IsInEnum();
    }
}
