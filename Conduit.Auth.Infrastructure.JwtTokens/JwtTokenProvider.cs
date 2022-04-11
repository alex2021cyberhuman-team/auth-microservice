using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Conduit.Auth.Domain.Services;
using Conduit.Auth.Domain.Services.ApplicationLayer.Users.Tokens;
using Conduit.Auth.Domain.Users;
using Conduit.Shared.Tokens;
using Microsoft.Extensions.Options;

namespace Conduit.Auth.Infrastructure.JwtTokens;

public class JwtTokenProvider : ITokenProvider
{
    private readonly JwtSecurityTokenHandler _handler;
    private readonly IIdManager _idManager;
    private readonly JwtTokenProviderOptions _options;

    public JwtTokenProvider(
        IOptions<JwtTokenProviderOptions> options,
        JwtSecurityTokenHandler handler,
        IIdManager idManager)
    {
        _handler = handler;
        _idManager = idManager;
        _options = options.Value;
    }

    public Task<TokenOutput> CreateTokenAsync(
        User user,
        CancellationToken cancellationToken = default)
    {
        var now = DateTime.UtcNow;
        var jti = _idManager.GenerateId().ToString();
        var accessToken = GetAccessToken(user, jti, now);
        var accessTokenString = _handler.WriteToken(accessToken);
        return Task.FromResult(new TokenOutput(accessTokenString));
    }

    private JwtSecurityToken GetAccessToken(
        User user,
        string jti,
        DateTime now)
    {
        var claims = user.GetCommonClaims()
            .Append(new(JwtRegisteredClaimNames.Jti, jti))
            .Append(new(JwtRegisteredClaimNames.Typ, "access"));
        var header = new JwtHeader(_options.GetSecurityCredentials());
        var payload = new JwtPayload(_options.Issuer, _options.Audience, claims,
            now, now.Add(_options.AccessTokenExpires), now);
        var accessToken = new JwtSecurityToken(header, payload);
        return accessToken;
    }
}
