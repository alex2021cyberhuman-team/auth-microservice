using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.ApplicationLayer.Users.Login;
using Conduit.Auth.ApplicationLayer.Users.Register;
using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Shared.AspNetCore.Controllers;
using Conduit.Shared.Outcomes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Conduit.Auth.PresentationLayer.WebApi.Controllers.Users;

[ApiController]
[Route("users")]
public class UsersController : SharedController
{
    public UsersController(
        IMediator mediator,
        ILoggerFactory loggerFactory) : base(mediator, loggerFactory)
    {
    }

    protected override string ControllerName { get; } = "UsersController";

    [HttpPost(Name = "registerUser")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> RegisterUser(
        [FromBody] RegisterUserRequest request,
        CancellationToken cancellationToken = default)
    {
        return await
            Send<Outcome<UserResponse>, RegisterUserRequest, UserResponse>(
                request, cancellationToken: cancellationToken);
    }

    [HttpPost("login", Name = "loginUser")]
    [ProducesResponseType(typeof(UserResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> LoginUser(
        [FromBody] LoginUserRequest request,
        CancellationToken cancellationToken = default)
    {
        return await
            Send<Outcome<UserResponse>, LoginUserRequest, UserResponse>(request,
                cancellationToken: cancellationToken);
    }
}
