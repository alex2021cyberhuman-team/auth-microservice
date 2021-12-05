using System;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Infrastructure.Dapper.Connection;
using Conduit.Auth.Infrastructure.Dapper.Extensions;
using Conduit.Auth.Infrastructure.Dapper.Users.Mappings;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Conduit.Auth.Infrastructure.Dapper.Users;

public class UsersFindByIdRepository : IUsersFindByIdRepository
{
    private readonly Compiler _compiler;
    private readonly IApplicationConnectionProvider _provider;

    public UsersFindByIdRepository(
        IApplicationConnectionProvider provider,
        Compiler compiler)
    {
        _provider = provider;
        _compiler = compiler;
    }

    #region IUsersFindByIdRepository Members

    public async Task<User?> FindByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        var connection =
            await _provider.CreateConnectionAsync(cancellationToken);
        var user = await connection.Get(_compiler).Query(UsersColumns.TableName)
            .Where(UsersColumns.Id, id)
            .FirstOrDefaultAsync<User>(cancellationToken: cancellationToken);
        return user;
    }

    #endregion
}
