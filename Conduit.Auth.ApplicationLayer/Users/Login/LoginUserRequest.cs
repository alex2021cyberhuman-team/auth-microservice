using Conduit.Auth.ApplicationLayer.Users.Shared;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using MediatR;

namespace Conduit.Auth.ApplicationLayer.Users.Login;

public class LoginUserRequest : IRequest<Outcome<UserResponse>>
{
    public LoginUserRequest(
        LoginUserModel user)
    {
        User = user;
    }

    public LoginUserModel User { get; set; }
}
