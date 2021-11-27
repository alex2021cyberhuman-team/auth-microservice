using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;

namespace Conduit.Auth.Domain.Services.ApplicationLayer.Users
{
    public interface ICurrentUserProvider
    {
        Task<Guid?> GetCurrentUserIdAsync(
            CancellationToken cancellationToken = default);

        Task<User?> GetCurrentUserAsync(
            CancellationToken cancellationToken = default);
    }
}
