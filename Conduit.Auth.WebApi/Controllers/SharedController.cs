using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Conduit.Auth.WebApi.Controllers
{
    public class SharedController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public SharedController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<IActionResult> Send<TResponse, TRequest, TResult>(
            TRequest request,
            Func<TResponse, IActionResult>? resultFactory = null,
            CancellationToken cancellationToken = default)
            where TRequest : IRequest<TResponse>
            where TResponse : Outcome<TResult>
        {
            var response = await _mediator.Send(request, cancellationToken);
            resultFactory ??= DefaultResultFactory<TResponse, TResult>;
            return resultFactory(response);
        }

        private IActionResult DefaultResultFactory<TResponse, TResult>(
            TResponse response)
            where TResponse : Outcome<TResult>
        {
            switch (response.Type)
            {
                case OutcomeType.Successful:
                    if (response.Result is null)
                    {
                        return NoContent();
                    }

                    return Ok(response.Result);
                case OutcomeType.Rejected:
                    if (response is not FluentRejectedOutcome<TResult>
                        rejectedOutcome)
                    {
                        return BadRequest();
                    }

                    foreach (var error in rejectedOutcome.ValidationResult
                        .Errors)
                    {
                        ModelState.AddModelError(
                            error.PropertyName,
                            error.ErrorMessage);
                    }

                    return BadRequest(ModelState);
                case OutcomeType.Failed:
                    return StatusCode(StatusCodes.Status500InternalServerError);
                case OutcomeType.Banned:
                    if (HttpContext.User.Identity?.IsAuthenticated ?? false)
                    {
                        return Forbid();
                    }

                    return Unauthorized();
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}
