using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Users;
using MongoDB.Driver;

namespace Conduit.Auth.InfrastructureLayer.MongoDB.Users.Dtos;

public static class DtosExtensions
{
    public static async Task<User?> ConvertToUserAsync(
        this IAsyncCursor<UserDto> asyncCursor,
        CancellationToken cancellationToken)
    {
        await asyncCursor.MoveNextAsync(cancellationToken);
        var userDto = asyncCursor.Current.FirstOrDefault();
        return userDto is null ? null : (User)userDto;
    }
}
