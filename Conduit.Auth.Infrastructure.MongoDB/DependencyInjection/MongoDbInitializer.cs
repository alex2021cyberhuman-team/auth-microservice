using System.Collections.Generic;
using System.Threading.Tasks;
using Conduit.Auth.Infrastructure.MongoDB.Connection;
using Conduit.Auth.Infrastructure.MongoDB.Users.Dtos;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Conduit.Auth.Infrastructure.MongoDB.DependencyInjection;

public class MongoDbInitializer
{
    private readonly IConnectionProvider _connectionProvider;

    public MongoDbInitializer(
        IConnectionProvider connectionProvider)
    {
        _connectionProvider = connectionProvider;
    }

    public async Task InitializeAsync()
    {
        RegisterConvention();
        await CreateUserIndexesAsync();
    }

    private static void RegisterConvention()
    {
        var conventionPack =
            new ConventionPack { new CamelCaseElementNameConvention() };
        ConventionRegistry.Register("camelCase", conventionPack, t => true);
    }

    private async Task CreateUserIndexesAsync()
    {
        var users = _connectionProvider.GetUsersCollection();
        var usersIndexes = new List<CreateIndexModel<UserDto>>
        {
            new(Builders<UserDto>.IndexKeys.Ascending(x => x.Username)),
            new(Builders<UserDto>.IndexKeys.Ascending(x => x.Email),
                new()
                {
                    Collation = new("en",
                        strength: CollationStrength.Secondary)
                })
        };
        var usersIndexManager = users.Indexes;
        await usersIndexManager.CreateManyAsync(usersIndexes);
    }
}
