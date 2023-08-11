using KUSYS.WebApi.Core.Application.Model;
using KUSYS.WebApi.Core.Domain;
using Microsoft.IdentityModel.Tokens;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace KUSYS.WebApi.Core.Tools
{
    public class JwtTokenGenerator
    {
        public static TokenResponse GenerateToken(string Username, int accountId ,Role role)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim("Username", Username));
            claims.Add(new Claim("AccountId", accountId.ToString()));
            claims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            claims.Add(new Claim("Role", role.ToString()));

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtTokenDefaults.Key));
            var credintial = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expireDate = DateTime.UtcNow.AddMinutes(JwtTokenDefaults.Expire);

            JwtSecurityToken jwt = new JwtSecurityToken(
                issuer: JwtTokenDefaults.ValidIssuer,
                audience: JwtTokenDefaults.ValidAudience,
                claims: claims.ToArray(),
                notBefore: DateTime.UtcNow,
                expires: expireDate,
                signingCredentials: credintial

                );
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            return new TokenResponse(handler.WriteToken(jwt), expireDate);

        }
    }
}
