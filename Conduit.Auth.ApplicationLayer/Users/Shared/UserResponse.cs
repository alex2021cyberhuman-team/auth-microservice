using Conduit.Auth.DomainLayer.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.DomainLayer.Users;

namespace Conduit.Auth.ApplicationLayer.Users.Shared;

public class UserResponse
{
    public UserResponse(
        User user,
        TokenOutput token)
    {
        User = new(user, token);
    }

    public UserResponseModel User { get; set; }
}
