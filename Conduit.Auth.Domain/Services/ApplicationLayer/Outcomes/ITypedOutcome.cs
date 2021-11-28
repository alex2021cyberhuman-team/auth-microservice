namespace Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes
{
    public interface ITypedOutcome
    {
        OutcomeType Type { get; }
    }
}
