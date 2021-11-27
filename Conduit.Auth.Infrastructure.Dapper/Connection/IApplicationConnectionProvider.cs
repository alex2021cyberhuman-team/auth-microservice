using System.Threading;
using System.Threading.Tasks;
using Npgsql;

namespace Conduit.Auth.Infrastructure.Dapper.Connection
{
    public interface IApplicationConnectionProvider
    {
        Task<NpgsqlConnection> CreateConnectionAsync(
            CancellationToken cancellationToken = default);
    }
}
