using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.ApplicationLayer.Users.GetCurrent;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.ApplicationLayer.Users.Update;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conduit.Auth.WebApi.Controllers.Users;

[ApiController]
[Route("user")]
public class UserController : SharedController
{
    public UserController(
        IMediator mediator,
        ILoggerFactory loggerFactory) : base(mediator, loggerFactory)
    {
    }

    protected override string ControllerName { get; } = "UserController";

    [Authorize]
    [HttpPut(Name = "updateUser")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> UpdateUser(
        [FromBody] UpdateUserRequest request,
        CancellationToken cancellationToken = default)
    {
        return await
            Send<Outcome<UserResponse>, UpdateUserRequest, UserResponse>(
                request, cancellationToken: cancellationToken);
    }

    [HttpGet(Name = "getCurrentUser")]
    [Authorize]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> GetCurrentUser(
        CancellationToken cancellationToken = default)
    {
        return await
            Send<Outcome<UserResponse>, GetCurrentUserRequest, UserResponse>(
                new(), cancellationToken: cancellationToken);
    }
}
