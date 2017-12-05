using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WisePay.Entities;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Users;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    [Authorize]
    public class UsersController : Controller
    {
        private UserManager<User> _userManager;
        private UsersService _usersService;

        public UsersController(UsersService usersService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _usersService = usersService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string query)
        {
            try
            {
                var users = await _usersService.GetAllByQuery(query);
                return await Task.FromResult(Json(users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Username = u.UserName
                })));
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.ServerError,
                    Message = e.Message
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var user = await _usersService.GetById(id);
            if (user != null)
            {
                return Json(new UserViewModel
                {
                    Id = user.Id,
                    Username = user.UserName
                });
            }
            else
            {
                return NotFound(new ErrorResponse
                {
                    Code = ErrorCode.NotFound,
                    Message = "User not found"
                });
            }
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMe()
        {
            try
            {
                var me = await _userManager.GetUserAsync(User);
                return Json(new CurrentUserViewModel
                {
                    Id = me.Id,
                    Username = me.UserName,
                    CardLastFourDigits = me.CardLastFourDigits
                });
            }
            catch (Exception e)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.ServerError,
                    Message = "Error while retrieving user information"
                });
            }
        }

        [HttpGet("/teams/{teamId}/users")]
        public async Task<IActionResult> GetUsersInTeam(int teamId)
        {
            try
            {
                var users = await _usersService.GetUsersInTeam(teamId);
                return Json(users.Select(u => new UserViewModel
                {
                    Id = u.Id,
                    Username = u.UserName
                }));
            } catch (Exception e)
            {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.ServerError,
                    Message = e.Message
                });
            }
        }
    }
}