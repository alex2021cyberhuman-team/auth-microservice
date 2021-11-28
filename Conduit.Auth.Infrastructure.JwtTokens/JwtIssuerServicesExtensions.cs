using System.IdentityModel.Tokens.Jwt;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Microsoft.Extensions.DependencyInjection;

namespace Conduit.Auth.Infrastructure.JwtTokens
{
    public static class JwtIssuerServicesExtensions
    {
        public static IServiceCollection AddJwtIssuerServices(
            this IServiceCollection services)
        {
            return services.AddSingleton<JwtSecurityTokenHandler>()
                .AddScoped<ITokenProvider, JwtTokenProvider>();
        }
    }
}
