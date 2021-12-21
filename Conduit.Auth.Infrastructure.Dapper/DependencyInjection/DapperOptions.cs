using Conduit.Auth.Infrastructure.Dapper.Connection;

namespace Conduit.Auth.Infrastructure.Dapper.DependencyInjection;

public class DapperOptions
{
    public NpgsqlConnectionOptions ConnectionOptions { get; set; } = new();
}
