using FluentValidation.Results;

namespace Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;

public class FluentRejectedOutcome<T> : Outcome<T>
{
    internal FluentRejectedOutcome(
        ValidationResult validationResult) : base(default, OutcomeType.Rejected)
    {
        ValidationResult = validationResult;
    }

    public ValidationResult ValidationResult { get; }
}
