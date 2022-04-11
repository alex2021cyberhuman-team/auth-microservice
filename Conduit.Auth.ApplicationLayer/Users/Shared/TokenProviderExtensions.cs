using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Outcomes;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Users;

namespace Conduit.Auth.ApplicationLayer.Users.Shared;

public static class TokenProviderExtensions
{
    public static async Task<Outcome<UserResponse>> CreateUserResponseAsync(
        this ITokenProvider tokenProvider,
        User user,
        CancellationToken cancellationToken = default)
    {
        var token =
            await tokenProvider.CreateTokenAsync(user, cancellationToken);
        var response = new UserResponse(user, token);
        return Outcome.New(OutcomeType.Successful, response);
    }
}
