﻿using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Infrastructure.MongoDB.Connection;
using Conduit.Auth.Infrastructure.MongoDB.Users.Dtos;
using MongoDB.Driver;

namespace Conduit.Auth.Infrastructure.MongoDB.Users;

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