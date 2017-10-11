using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WisePay.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using WisePay.Web.Auth;
using Microsoft.IdentityModel.Tokens;

namespace WisePay.Web.Controllers
{
    [Route("api/token")]
    public class AuthTokenController : Controller
    {
        private UserManager<User> _userManager;

        public AuthTokenController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateToken(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return BadRequest("Invalid email");
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, password);
            if (!isPasswordCorrect)
            {
                return BadRequest("Invalid password");
            }

            var claims = await _userManager.GetClaimsAsync(user);
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                            issuer: AuthOptions.Issuer,
                            audience: AuthOptions.Audience,
                            notBefore: now,
                            claims: claims,
                            expires: now.Add(TimeSpan.FromDays(AuthOptions.Lifetime)),
                            signingCredentials: new SigningCredentials(AuthOptions.SymmetricSecurityKey, SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt
            };

            return Json(response);
        }
    }
}