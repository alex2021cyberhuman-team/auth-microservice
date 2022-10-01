using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.DomainLayer.Users;
using Conduit.Auth.DomainLayer.Users.Repositories;
using Conduit.Auth.InfrastructureLayer.MongoDB.Connection;
using Conduit.Auth.InfrastructureLayer.MongoDB.Users.Dtos;
using MongoDB.Driver;

namespace Conduit.Auth.InfrastructureLayer.MongoDB.Users;

public class UsersWriteRepository : IUsersWriteRepository
{
    private readonly IConnectionProvider _connectionProvider;

    public UsersWriteRepository(
        IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<User> CreateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var users = _connectionProvider.GetUsersCollection();
        await users.InsertOneAsync(new(user), null, cancellationToken);
        return user;
    }

    public async Task<User> UpdateAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var users = _connectionProvider.GetUsersCollection();
        await users.ReplaceOneAsync(
            Builders<UserDto>.Filter.Eq(x => x.Id, user.Id), new(user),
            new ReplaceOptions(), cancellationToken);
        return user;
    }
}
