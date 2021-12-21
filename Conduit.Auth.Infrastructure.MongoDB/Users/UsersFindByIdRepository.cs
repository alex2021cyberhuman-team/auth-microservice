using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Infrastructure.MongoDB.Connection;
using Conduit.Auth.Infrastructure.MongoDB.Users.Dtos;
using MongoDB.Driver;

namespace Conduit.Auth.Infrastructure.MongoDB.Users;

public class UsersFindByIdRepository : IUsersFindByIdRepository
{
    private readonly IConnectionProvider _connectionProvider;

    public UsersFindByIdRepository(
        IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task<User?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var users = _connectionProvider.GetUsersCollection();
        var asyncCursor = await users.FindAsync(x => x.Id == id,
            cancellationToken: cancellationToken);

        return await asyncCursor.ConvertToUserAsync(cancellationToken);
    }
}
