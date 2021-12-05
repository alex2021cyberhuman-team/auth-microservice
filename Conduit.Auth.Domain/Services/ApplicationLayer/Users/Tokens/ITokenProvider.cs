using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;

namespace Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;

public interface ITokenProvider
{
    Task<TokenOutput> CreateTokenAsync(
        User user,
        CancellationToken cancellationToken = default);
}
