using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Users;
using Conduit.Auth.DomainLayer.Users.Repositories;
using Conduit.Auth.InfrastructureLayer.MongoDB.Connection;
using Conduit.Auth.InfrastructureLayer.MongoDB.Users.Dtos;
using MongoDB.Driver;

namespace Conduit.Auth.InfrastructureLayer.MongoDB.Users;

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
