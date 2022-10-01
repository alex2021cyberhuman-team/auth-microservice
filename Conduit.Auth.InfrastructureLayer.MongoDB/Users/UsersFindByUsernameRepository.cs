using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Users;
using Conduit.Auth.DomainLayer.Users.Repositories;
using Conduit.Auth.InfrastructureLayer.MongoDB.Connection;
using Conduit.Auth.InfrastructureLayer.MongoDB.Users.Dtos;
using MongoDB.Driver;

namespace Conduit.Auth.InfrastructureLayer.MongoDB.Users;

public class UsersFindByUsernameRepository : IUsersFindByUsernameRepository
{
    private readonly IConnectionProvider _connectionProvider;

    public UsersFindByUsernameRepository(
        IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<User?> FindByUsernameAsync(
        string username,
        CancellationToken cancellationToken = default)
    {
        var users = _connectionProvider.GetUsersCollection();
        var asyncCursor = await users.FindAsync(x => x.Username == username,
            cancellationToken: cancellationToken);

        return await asyncCursor.ConvertToUserAsync(cancellationToken);
    }
}
