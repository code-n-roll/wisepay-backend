using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WisePay.Entities;

namespace WisePay.Web.Auth
{
    public class AuthTokenService
    {
        private UserManager<User> _userManager;

        public AuthTokenService(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<string> GenerateToken(User user)
        {
            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                            issuer: AuthConfig.Issuer,
                            audience: AuthConfig.Audience,
                            notBefore: now,
                            claims: claims,
                            expires: now.Add(TimeSpan.FromDays(AuthConfig.Lifetime)),
                            signingCredentials: new SigningCredentials(AuthConfig.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
    }
}
