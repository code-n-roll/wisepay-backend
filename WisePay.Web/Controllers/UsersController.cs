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
using WisePay.Web.Teams;
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
        private TeamsService _teamsService;

        public UsersController(
            UsersService usersService,
            TeamsService teamsService,
            UserManager<User> userManager)
        {
            _userManager = userManager;
            _usersService = usersService;
            _teamsService = teamsService;
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

        [HttpGet("me/teams")]
        public async Task<IActionResult> GetMyTeams()
        {
            try {
                var me = await _userManager.GetUserAsync(User);
                var teams = await _teamsService.GetUserTeams(me.Id);

                return Json(teams.Select(t => new TeamShortInfoViewModel {
                    Id = t.Id,
                    Name = t.Name
                }));
            }
            catch (Exception e) {
                return BadRequest(new ErrorResponse
                {
                    Code = ErrorCode.ServerError,
                    Message = e.Message
                });
            }
        }
    }
}