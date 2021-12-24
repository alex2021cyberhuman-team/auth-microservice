using System;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Infrastructure.MongoDB.Connection;
using Conduit.Auth.Infrastructure.MongoDB.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.Infrastructure.MongoDB.DependencyInjection;

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
            .AddTransient<MongoDbInitializer>()
            .AddHealthChecks()
            .AddMongoDb(GetConnectionString(configure))
            .Services;
    }

    private static string GetConnectionString(
        this Action<MongoDbConnectionOptions> configure)
    {
        var options = new MongoDbConnectionOptions();
        configure(options);
        return options.ConnectionString;
    }
}