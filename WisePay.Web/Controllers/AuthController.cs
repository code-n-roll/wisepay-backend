using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using WisePay.Entities;
using WisePay.Web.Auth;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Internals;
using WisePay.Web.Auth.Models;
using System.Linq;
using System.Collections.Generic;

namespace WisePay.Web.Controllers
{
    [Route("api")]
    public class AuthController : Controller
    {
        private UserManager<User> _userManager;
        private AuthTokenService _tokenService;

        public AuthController(UserManager<User> userManager, AuthTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("sign_in")]
        public async Task<IActionResult> GenerateToken([FromBody]LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                throw new ApiException(400, "Invalid email", ErrorCode.InvalidCredentials);
            }

            var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, model.Password);
            if (!isPasswordCorrect)
            {
                throw new ApiException(400, "Invalid password", ErrorCode.InvalidCredentials);
            }

            var token = await _tokenService.GenerateToken(user);

            var response = new
            {
                access_token = token,
                email = user.Email
            };

            return Json(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterModel registerModel)
        {
            if (registerModel.Password != registerModel.PasswordConfirmation)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.InvalidCredentials,
                    Message = "Passwords don't match"
                });
            }

            var newUser = new User()
            {
                Email = registerModel.Email,
                UserName = registerModel.Email
            };

            var result = await _userManager.CreateAsync(newUser, registerModel.Password);

            if (!result.Succeeded)
            {
                return GetErrorResult(result);
            }

            var response = new
            {
                access_token = await _tokenService.GenerateToken(newUser),
                email = newUser.Email
            };

            return Ok(response);
        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.ServerError
                });
            }

            if (result.Errors != null)
            {
                var errorResponse = new ErrorResponse();

                if (result.Errors.Count() == 1)
                {
                    errorResponse.Code = ErrorCode.AuthError;
                    errorResponse.Message = result.Errors.ElementAt(0).Description;
                }
                else
                {
                    errorResponse.Code = ErrorCode.MultipleErrors;
                    var innerErrors = new List<InnerError>();

                    foreach (var error in result.Errors)
                    {
                        innerErrors.Add(new InnerError
                        {
                            Code = ErrorCode.AuthError,
                            Message = error.Description
                        });
                    }

                    errorResponse.InnerErrors = innerErrors;
                }

                return BadRequest(errorResponse);
            }
            else
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.ServerError
                });
            }
        }
    }
}
