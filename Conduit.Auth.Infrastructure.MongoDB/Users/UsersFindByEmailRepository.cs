using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Infrastructure.MongoDB.Connection;
using Conduit.Auth.Infrastructure.MongoDB.Users.Dtos;
using MongoDB.Driver;

namespace Conduit.Auth.Infrastructure.MongoDB.Users;

public class UsersFindByEmailRepository : IUsersFindByEmailRepository
{
    private readonly IConnectionProvider _connectionProvider;

    public UsersFindByEmailRepository(
        IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<User?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var users = _connectionProvider.GetUsersCollection();
        var asyncCursor = await users.FindAsync(x => x.Email == email,
            new FindOptions<UserDto>
            {
                Collation = new("en", strength: CollationStrength.Secondary)
            }, cancellationToken);

        return await asyncCursor.ConvertToUserAsync(cancellationToken);
    }
}
