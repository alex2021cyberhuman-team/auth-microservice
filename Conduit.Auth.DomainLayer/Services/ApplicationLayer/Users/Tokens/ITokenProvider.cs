using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Users;

namespace Conduit.Auth.DomainLayer.Services.ApplicationLayer.Users.Tokens;

public interface ITokenProvider
{
    Task<TokenOutput> CreateTokenAsync(
        User user,
        CancellationToken cancellationToken = default);
}
