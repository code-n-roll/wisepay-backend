﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using WisePay.Entities;
using WisePay.Web.Auth;

namespace WisePay.Web.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private UserManager<User> _userManager;

        public AuthController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token(string email, string password)
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

            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: claims,
                expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                email = email
            };

            return Json(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (registerModel.Password != registerModel.PasswordConfirmation)
            {
                return BadRequest("Passwords don't match");
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

            return Ok();
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email)
            };
            var claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

        private IActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return StatusCode(500);
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }

                if (ModelState.IsValid)
                {
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }

    }
}