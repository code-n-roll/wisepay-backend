using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WisePay.Entities;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using WisePay.Web.Auth;
using Microsoft.IdentityModel.Tokens;
using WisePay.Web.Core.ClientInteraction;

namespace WisePay.Web.Controllers
{
    [Route("api/token")]
    public class AuthTokenController : Controller
    {
        private UserManager<User> _userManager;
        private AuthTokenService _tokenService;

        public AuthTokenController(UserManager<User> userManager, AuthTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody]LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.InvalidCredentials,
                    Message = "Invalid email"
                });
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordCorrect)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.InvalidCredentials,
                    Message = "Invalid password"
                });
            }

            var token = await _tokenService.GenerateToken(user);

            var response = new
            {
                access_token = token,
                email = user.Email
            };

            return Json(response);
        }
    }
}