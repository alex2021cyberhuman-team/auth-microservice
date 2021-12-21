using Conduit.Auth.Infrastructure.MongoDB.Users.Dtos;
using MongoDB.Driver;

namespace Conduit.Auth.Infrastructure.MongoDB.Connection;

public interface IConnectionProvider
{
    MongoClient GetClient();
    IMongoDatabase GetUsersDatabase();
    IMongoCollection<UserDto> GetUsersCollection();
}
