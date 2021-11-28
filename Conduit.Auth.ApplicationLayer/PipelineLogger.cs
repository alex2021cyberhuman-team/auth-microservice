using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Conduit.Auth.ApplicationLayer
{
    public class PipelineLogger<TRequest, TResponse>
        : IPipelineBehavior<TRequest, TResponse>
        where TRequest : notnull where TResponse : ITypedOutcome
    {
        private readonly ILogger<PipelineLogger<TRequest, TResponse>> _logger;

        public PipelineLogger(
            ILogger<PipelineLogger<TRequest, TResponse>> logger)
        {
            _logger = logger;
        }

        #region IPipelineBehavior<TRequest,TResponse> Members

        public async Task<TResponse> Handle(
            TRequest request,
            CancellationToken cancellationToken,
            RequestHandlerDelegate<TResponse> next)
        {
            _logger.LogInformation(
                EventIds.StartHandling,
                "Start handling request: {Request}",
                request);

            var result = await next();

            LogResponse(request, result.Type);

            return result;
        }

        #endregion

        private void LogResponse(TRequest request, OutcomeType outcomeType)
        {
            switch (outcomeType)
            {
                case OutcomeType.Successful:
                    _logger.LogInformation(
                        EventIds.SuccessfulHandling,
                        "Successful handling request: {Request}",
                        request);
                    break;
                case OutcomeType.Rejected:
                    _logger.LogInformation(
                        EventIds.RejectedHandling,
                        "Rejected request: {Request}",
                        request);
                    break;
                case OutcomeType.Failed:
                    _logger.LogError(
                        EventIds.FailedHandling,
                        "Failed request: {Request}",
                        request);
                    break;
                case OutcomeType.Banned:
                    _logger.LogInformation(
                        EventIds.BannedHandling,
                        "Banned request: {Request}",
                        request);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(
                        nameof(outcomeType),
                        "outcomeType is invalid");
            }
        }

        #region Nested type: EventIds

        public static class EventIds
        {
            public static EventId StartHandling => new(5221, "StartHandling");

            public static EventId SuccessfulHandling =>
                new(5211, "SuccessfulHandling");

            public static EventId RejectedHandling =>
                new(5212, "RejectedHandling");

            public static EventId FailedHandling => new(5213, "FailedHandling");

            public static EventId BannedHandling => new(5214, "BannedHandling");
        }

        #endregion
    }
}
