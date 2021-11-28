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

namespace Conduit.Auth.Infrastructure.Dapper.Users
{
    public class UsersWriteRepository : IUsersWriteRepository
    {
        private readonly Compiler _compiler;
        private readonly IUsersFindByIdRepository _findById;
        private readonly IApplicationConnectionProvider _provider;

        public UsersWriteRepository(
            IApplicationConnectionProvider provider,
            Compiler compiler,
            IUsersFindByIdRepository findById)
        {
            _provider = provider;
            _compiler = compiler;
            _findById = findById;
        }

        #region IUsersWriteRepository Members

        public async Task<User> CreateAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            var connection =
                await _provider.CreateConnectionAsync(cancellationToken);
            await connection.Get(_compiler)
                .Query(UsersColumns.TableName)
                .InsertAsync(
                    user.AsColumns(),
                    cancellationToken: cancellationToken);

            return (await _findById.FindByIdAsync(user.Id, cancellationToken))!;
        }

        public async Task<User> UpdateAsync(
            User user,
            CancellationToken cancellationToken = default)
        {
            var connection =
                await _provider.CreateConnectionAsync(cancellationToken);
            var updatedRows = await connection.Get(_compiler)
                .Query(UsersColumns.TableName)
                .Where(UsersColumns.Id, user.Id)
                .UpdateAsync(
                    user.AsColumns(),
                    cancellationToken: cancellationToken);
            return updatedRows switch
            {
                0 => throw new InvalidOperationException(
                    "No one row has been updated."),
                > 1 => throw new InvalidOperationException(
                    "Several rows have been updated."),
                _ => (await _findById.FindByIdAsync(
                    user.Id,
                    cancellationToken))!
            };
        }

        #endregion
    }
}
