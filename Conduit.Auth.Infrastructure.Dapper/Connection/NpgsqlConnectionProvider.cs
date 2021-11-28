using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Infrastructure.Dapper.DependencyInjection;
using Microsoft.Extensions.Options;
using Npgsql;

namespace Conduit.Auth.Infrastructure.Dapper.Connection
{
    public class NpgsqlConnectionProvider
        : IApplicationConnectionProvider,
            IAsyncDisposable
    {
        private readonly IOptionsMonitor<DapperOptions> _optionsMonitor;

        private NpgsqlConnection? _currentScopeConnection;

        public NpgsqlConnectionProvider(
            IOptionsMonitor<DapperOptions> optionsMonitor)
        {
            _optionsMonitor = optionsMonitor;
        }

        #region IApplicationConnectionProvider Members

        public async Task<NpgsqlConnection> CreateConnectionAsync(
            CancellationToken cancellationToken = default)
        {
            var options = _optionsMonitor.CurrentValue;
            var connectionsString = options.ConnectionOptions.ConnectionString;
            _currentScopeConnection = await GetConnectionAsync(
                _currentScopeConnection,
                connectionsString,
                cancellationToken);
            return _currentScopeConnection;
        }

        #endregion

        #region IAsyncDisposable Members

        public async ValueTask DisposeAsync()
        {
            if (_currentScopeConnection is not null)
            {
                await _currentScopeConnection.DisposeAsync();
            }

            GC.SuppressFinalize(this);
        }

        #endregion

        private static async Task<NpgsqlConnection> GetConnectionAsync(
            NpgsqlConnection? connection,
            string connectionsString,
            CancellationToken cancellationToken)
        {
            if (connection is null)
            {
                return await New();
            }

            switch (connection.FullState)
            {
                case ConnectionState.Closed:
                    await connection.OpenAsync(cancellationToken);
                    return connection;
                case ConnectionState.Broken:
                    return await New();
                case ConnectionState.Open:
                case ConnectionState.Connecting:
                case ConnectionState.Open | ConnectionState.Executing:
                case ConnectionState.Open | ConnectionState.Fetching:
                    return connection;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            async Task<NpgsqlConnection> New()
            {
                var newConnection = new NpgsqlConnection(connectionsString);
                await newConnection.OpenAsync(cancellationToken);
                return newConnection;
            }
        }
    }
}
