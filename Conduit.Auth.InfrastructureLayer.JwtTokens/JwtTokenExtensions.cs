using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Conduit.Auth.DomainLayer.Users;
using Conduit.Shared.Tokens;
using Microsoft.IdentityModel.Tokens;
using OpenIddict.Abstractions;

namespace Conduit.Auth.InfrastructureLayer.JwtTokens;

public static class JwtTokenExtensions
{
    public static SecurityKey GetSecurityKey(
        this JwtTokenProviderOptions opt)
    {
        return new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(opt.SecurityKey));
    }
    //
    // Enumerable.Empty<Claim>()
    // .Append(new(OpenIddictConstants.Claims.Subject, await _userManager.GetUserIdAsync(user)))
    //     .Append(new(OpenIddictConstants.Claims.Email, await _userManager.GetEmailAsync(user)))
    //     .Append(new(OpenIddictConstants.Claims.Name, await _userManager.GetUserNameAsync(user)))
    //     .Concat(
    //         (await _userManager.GetRolesAsync(user))
    //     .Select(role => new Claim(OpenIddictConstants.Claims.Role, role)));

    public static IEnumerable<Claim> GetCommonClaims(
        this User user)
    {
        yield return new(OpenIddictConstants.Claims.Subject, user.Id.ToString());
        yield return new(OpenIddictConstants.Claims.Name, user.Username);
        yield return new(OpenIddictConstants.Claims.Email, user.Email, ClaimValueTypes.Email);
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
