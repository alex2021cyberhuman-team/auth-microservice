using FluentValidation.Results;

namespace Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes
{
    public static class Outcome
    {
        public static Outcome<T> New<T>(
            OutcomeType type = OutcomeType.Successful,
            T? result = default)
        {
            return new(result, type);
        }

        public static FluentRejectedOutcome<T> Reject<T>(
            ValidationResult validationResult)
        {
            return new(validationResult);
        }
    }

    public class Outcome<T>
    {
        internal Outcome(
            T? result,
            OutcomeType type)
        {
            Result = result;
            Type = type;
        }

        public T? Result { get; }

        public OutcomeType Type { get; }

        public static implicit operator bool(
            Outcome<T> outcome)
        {
            return outcome.Type == OutcomeType.Successful;
        }
    }
}
