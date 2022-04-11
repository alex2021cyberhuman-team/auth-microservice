using Conduit.Auth.Infrastructure.MongoDB.Users.Dtos;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace Conduit.Auth.Infrastructure.MongoDB.Connection;

public class ConnectionProvider : IConnectionProvider
{
    private readonly IOptionsMonitor<MongoDbConnectionOptions> _optionsMonitor;
    private MongoClient? _client;
    private IMongoDatabase? _usersDatabase;

    public ConnectionProvider(
        IOptionsMonitor<MongoDbConnectionOptions> optionsMonitor)
    {
        _optionsMonitor = optionsMonitor;
    }

    private MongoDbConnectionOptions Options => _optionsMonitor.CurrentValue;

    public MongoClient GetClient()
    {
        return _client ??= new(Options.ConnectionString);
    }

    public IMongoDatabase GetUsersDatabase()
    {
        return _usersDatabase ??=
            GetClient().GetDatabase(Options.UsersDatabase);
    }

    public IMongoCollection<UserDto> GetUsersCollection()
    {
        return GetUsersDatabase().GetCollection<UserDto>(CollectionNames.Users);
    }
}
