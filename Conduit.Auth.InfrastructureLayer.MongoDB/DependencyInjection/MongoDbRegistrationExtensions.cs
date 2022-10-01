using System;
using Conduit.Auth.DomainLayer.Services.DataAccess;
using Conduit.Auth.DomainLayer.Users.Repositories;
using Conduit.Auth.InfrastructureLayer.MongoDB.Connection;
using Conduit.Auth.InfrastructureLayer.MongoDB.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.InfrastructureLayer.MongoDB.DependencyInjection;

public static class MongoDbRegistrationExtensions
{
    public static IServiceCollection AddMongoWithHealthChecks(
        this IServiceCollection services,
        Action<MongoDbConnectionOptions> configure)
    {
        return services.Configure(configure)
            .AddSingleton<IUsersFindByEmailRepository,
                UsersFindByEmailRepository>()
            .AddSingleton<IUsersFindByUsernameRepository,
                UsersFindByUsernameRepository>()
            .AddSingleton<IUsersFindByIdRepository, UsersFindByIdRepository>()
            .AddSingleton<IUsersWriteRepository, UsersWriteRepository>()
            .AddSingleton<IConnectionProvider, ConnectionProvider>()
            .AddScoped<IUnitOfWork, ServiceProviderUnitOfWork>()
            .AddTransient<MongoDbInitializer>().AddHealthChecks()
            .AddMongoDb(GetConnectionString(configure)).Services;
    }

    private static string GetConnectionString(
        this Action<MongoDbConnectionOptions> configure)
    {
        var options = new MongoDbConnectionOptions();
        configure(options);
        return options.ConnectionString;
    }
}
