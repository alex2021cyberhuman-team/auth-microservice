namespace Conduit.Auth.InfrastructureLayer.MongoDB.Connection;

public class MongoDbConnectionOptions
{
    public string ConnectionString { get; set; } = string.Empty;

    public string UsersDatabase { get; set; } = "users-database";
}
