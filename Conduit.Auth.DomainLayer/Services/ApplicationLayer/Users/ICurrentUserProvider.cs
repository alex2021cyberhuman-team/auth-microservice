using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Users;

namespace Conduit.Auth.DomainLayer.Services.ApplicationLayer.Users;

public interface ICurrentUserProvider
{
    Task<Guid?> GetCurrentUserIdAsync(
        CancellationToken cancellationToken = default);

    Task<User?> GetCurrentUserAsync(
        CancellationToken cancellationToken = default);
}
