using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Users;

namespace Conduit.Auth.ApplicationLayer.Users.Shared
{
    public class UserResponse
    {
        public UserResponse(User user, TokenOutput token)
        {
            User = new(user, token);
        }

        public UserResponseModel User { get; set; }
    }
}
