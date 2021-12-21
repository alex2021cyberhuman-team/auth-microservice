using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Infrastructure.MongoDB.Extensions;
using Conduit.Auth.Infrastructure.MongoDB.Connection;
using Conduit.Auth.Infrastructure.MongoDB.Users.Mappings;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Conduit.Auth.Infrastructure.MongoDB.Users;

public class UsersFindByEmailRepository : IUsersFindByEmailRepository
{
    private readonly Compiler _compiler;
    private readonly IApplicationConnectionProvider _provider;

    public UsersFindByEmailRepository(
        IApplicationConnectionProvider provider,
        Compiler compiler)
    {
        _provider = provider;
        _compiler = compiler;
    }

    #region IUsersFindByEmailRepository Members

    public async Task<User?> FindByEmailAsync(
        string email,
        CancellationToken cancellationToken = default)
    {
        var connection =
            await _provider.CreateConnectionAsync(cancellationToken);
        var user = await connection.Get(_compiler).Query(UsersColumns.TableName)
            .Where(UsersColumns.EmailLower, email.ToLower())
            .FirstOrDefaultAsync<User>(cancellationToken: cancellationToken);
        return user;
    }

    #endregion
}
