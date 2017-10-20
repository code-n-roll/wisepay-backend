using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WisePay.Entities;
using WisePay.Web.Auth;
using WisePay.Web.Core.ClientInteraction;

namespace WisePay.Web.Controllers
{
    [Route("api/account")]
    public class AccountController : Controller
    {
        private UserManager<User> _userManager;
        private AuthTokenService _tokenService;

        public AccountController(UserManager<User> userManager, AuthTokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
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
                    var innerErrors = new List<ErrorResponse>();

                    foreach (var error in result.Errors)
                    {
                        innerErrors.Add(new ErrorResponse
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