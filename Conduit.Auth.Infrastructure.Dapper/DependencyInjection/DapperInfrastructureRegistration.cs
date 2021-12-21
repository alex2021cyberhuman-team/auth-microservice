using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services.DataAccess;
using Conduit.Auth.Domain.Users.Repositories;
using Conduit.Auth.Infrastructure.Dapper.Connection;
using Conduit.Auth.Infrastructure.Dapper.Migrations;
using Conduit.Auth.Infrastructure.Dapper.Users;
using Conduit.Auth.Infrastructure.Dapper.Users.Mappings;
using Dapper.FluentMap;
using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using SqlKata.Compilers;

namespace Conduit.Auth.Infrastructure.Dapper.DependencyInjection;

public static class DapperInfrastructureRegistration
{
    public static IServiceCollection AddDapper(
        this IServiceCollection services,
        Action<DapperOptions> action)
    {
        FluentMapper.Initialize(conf => conf.AddMap(new UsersTableMap()));
        var options = GetOptions(action);
        services.Configure(action).AddSingleton<Compiler, PostgresCompiler>()
            .AddScoped<IApplicationConnectionProvider,
                NpgsqlConnectionProvider>()
            .AddScoped<IUnitOfWork, ServiceProviderUnitOfWork>()
            .AddScoped<IUsersFindByUsernameRepository,
                UsersFindByUsernameRepository>()
            .AddScoped<IUsersFindByIdRepository, UsersFindByIdRepository>()
            .AddScoped<IUsersWriteRepository, UsersWriteRepository>()
            .AddScoped<IUsersFindByEmailRepository,
                UsersFindByEmailRepository>().AddFluentMigratorCore()
            .ConfigureRunner(rb =>
                rb.AddPostgres()
                    .WithGlobalConnectionString(options.ConnectionOptions
                        .ConnectionString)
                    .ScanIn(Assembly.GetExecutingAssembly()).For.Migrations())
            .AddTransient<MigrationService>().AddHealthChecks()
            .AddNpgSql(options.ConnectionOptions.ConnectionString);
#if DEBUG
        if (!CheckRepositoriesFromDomain(services))
        {
            throw new InvalidOperationException(
                "Not all repositories have been registered");
        }
#endif
        return services;
    }

    public static async Task InitializeDatabaseAsync(
        this IServiceScope scope)
    {
        var migrations = scope.ServiceProvider
            .GetRequiredService<MigrationService>();
        await migrations.InitializeAsync();
    }

    private static DapperOptions GetOptions(
        Action<DapperOptions> action)
    {
        var options = new DapperOptions();
        action(options);
        return options;
    }

    private static bool CheckRepositoriesFromDomain(
        IEnumerable<ServiceDescriptor> descriptors)
    {
        var repositoryInterfacesFromDomain = typeof(IRepository).Assembly
            .GetTypes()
            .Where(type => type.IsInterface && type != typeof(IRepository))
            .Where(type => type.IsAssignableTo(typeof(IRepository)))
            .ToHashSet();
        var repositoryClassesFromThisAssembly = descriptors
            .Select(descriptor => descriptor.ImplementationType)
            .Where(type =>
                type is not null &&
                type.IsAssignableTo(typeof(IRepository)) &&
                type.IsClass).Where(repositoryClass =>
                repositoryInterfacesFromDomain.Any(repositoryInterface =>
                    repositoryInterface.IsAssignableFrom(repositoryClass)))
            .ToHashSet();
        return repositoryInterfacesFromDomain.Count ==
               repositoryClassesFromThisAssembly.Count;
    }
}
