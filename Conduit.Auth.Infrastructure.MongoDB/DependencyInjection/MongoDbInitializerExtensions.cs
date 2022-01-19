using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.Infrastructure.MongoDB.DependencyInjection;

public static class MongoDbInitializerExtensions
{
    public static async Task<IServiceScope> InitializeMongoDbAsync(
        this IServiceScope scope)
    {
        var initializer =
            scope.ServiceProvider.GetRequiredService<MongoDbInitializer>();
        await initializer.InitializeAsync();
        return scope;
    }
}
