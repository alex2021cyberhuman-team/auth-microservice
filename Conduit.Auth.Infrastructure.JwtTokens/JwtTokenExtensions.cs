using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using Conduit.Auth.Domain.Users;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;

namespace Conduit.Auth.Infrastructure.JwtTokens
{
    public static class JwtTokenExtensions
    {
        public static SecurityKey GetSecurityKey(
            this JwtTokenProviderOptions opt)
        {
            return new SymmetricSecurityKey(
                Encoding.ASCII.GetBytes(opt.SecurityKey));
        }


        public static IEnumerable<Claim> GetCommonClaims(this User user)
        {
            yield return new(JwtRegisteredClaimNames.Sub, user.Id.ToString());
            yield return new(JwtRegisteredClaimNames.Name, user.Username);
            yield return new(
                JwtRegisteredClaimNames.Email,
                user.Email,
                ClaimValueTypes.Email);
        }

        public static SigningCredentials GetSecurityCredentials(
            this JwtTokenProviderOptions opt)
        {
            return new(opt.GetSecurityKey(), opt.SecurityKeyAlgorithm);
        }
    }
}
