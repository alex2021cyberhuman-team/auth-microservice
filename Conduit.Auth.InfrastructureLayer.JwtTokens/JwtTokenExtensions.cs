using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Conduit.Auth.DomainLayer.Users;
using Conduit.Shared.Tokens;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Auth.InfrastructureLayer.JwtTokens;

public static class JwtTokenExtensions
{
    public static SecurityKey GetSecurityKey(
        this JwtTokenProviderOptions opt)
    {
        return new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(opt.SecurityKey));
    }


    public static IEnumerable<Claim> GetCommonClaims(
        this User user)
    {
        yield return new(ClaimTypes.NameIdentifier, user.Id.ToString());
        yield return new(ClaimTypes.Name, user.Username);
        yield return new(ClaimTypes.Email, user.Email, ClaimValueTypes.Email);
    }

    public static SigningCredentials GetSecurityCredentials(
        this JwtTokenProviderOptions opt)
    {
        return new(opt.GetSecurityKey(), opt.SecurityKeyAlgorithm);
    }
}
