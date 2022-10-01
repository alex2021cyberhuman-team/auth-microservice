using Conduit.Auth.InfrastructureLayer.MongoDB.Users.Dtos;
using MongoDB.Driver;

namespace Conduit.Auth.InfrastructureLayer.MongoDB.Connection;

public interface IConnectionProvider
{
    MongoClient GetClient();
    IMongoDatabase GetUsersDatabase();
    IMongoCollection<UserDto> GetUsersCollection();
}
