using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Users;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Infrastructure.Dapper.Connection;
using Conduit.Auth.Infrastructure.Dapper.Extensions;
using Conduit.Auth.Infrastructure.Dapper.Users.Mappings;
using SqlKata.Compilers;
using SqlKata.Execution;

namespace Conduit.Auth.Infrastructure.Dapper.Users
{
    public class UsersFindByUsernameRepository : IUsersFindByUsernameRepository
    {
        private readonly Compiler _compiler;
        private readonly IApplicationConnectionProvider _provider;

        public UsersFindByUsernameRepository(
            IApplicationConnectionProvider provider,
            Compiler compiler)
        {
            _provider = provider;
            _compiler = compiler;
        }

        #region IUsersFindByUsernameRepository Members

        public async Task<User?> FindByUsernameAsync(
            string username,
            CancellationToken cancellationToken = default)
        {
            var connection =
                await _provider.CreateConnectionAsync(cancellationToken);
            var user = await connection.Get(_compiler)
                .Query(UsersColumns.TableName)
                .Where(UsersColumns.Username, username)
                .FirstOrDefaultAsync<User>(
                    cancellationToken: cancellationToken);
            return user;
        }

        #endregion
    }
}
