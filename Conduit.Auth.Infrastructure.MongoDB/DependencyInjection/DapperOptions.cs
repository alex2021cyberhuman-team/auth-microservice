using Conduit.Auth.Infrastructure.MongoDB.Connection;

namespace Conduit.Auth.Infrastructure.MongoDB.DependencyInjection;

public class DapperOptions
{
    public NpgsqlConnectionOptions ConnectionOptions { get; set; } = new();
}
