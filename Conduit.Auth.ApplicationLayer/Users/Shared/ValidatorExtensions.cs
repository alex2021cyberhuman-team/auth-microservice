using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users;
using Conduit.Auth.Domain.Users;

namespace Conduit.Auth.ApplicationLayer.Users.Shared;

public static class ValidatorExtensions
{
    public static async Task<bool> CheckCurrentUser(
        this User? user,
        ICurrentUserProvider? currentUserProvider = null,
        CancellationToken cancellation = default)
    {
        if (currentUserProvider is null)
        {
            return user is null;
        }

        var currentUserId =
            await currentUserProvider.GetCurrentUserIdAsync(cancellation);

        return user is null || user.Id == currentUserId;
    }
}
