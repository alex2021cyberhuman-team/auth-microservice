namespace Conduit.Auth.Infrastructure.MongoDB.Connection;

public class MongoDbConnectionOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public string UsersDatabase { get; set; } = "users-database";
}
