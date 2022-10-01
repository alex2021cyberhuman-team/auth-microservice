using System.IdentityModel.Tokens.Jwt;
using Conduit.Auth.DomainLayer.Services.ApplicationLayer.Users.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.InfrastructureLayer.JwtTokens;

public static class JwtIssuerServicesExtensions
{
    public static IServiceCollection AddJwtIssuerServices(
        this IServiceCollection services)
    {
        return services.AddSingleton<JwtSecurityTokenHandler>()
            .AddScoped<ITokenProvider, JwtTokenProvider>();
    }
}
