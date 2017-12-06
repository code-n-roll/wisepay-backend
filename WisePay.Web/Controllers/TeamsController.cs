using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Teams;
using WisePay.Web.Users;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Teams")]
    // [Authorize]
    public class TeamsController : Controller
    {
        private TeamsService _teamsService;
        private UsersService _usersService;
        private IMapper _mapper;

        public TeamsController(TeamsService teamsService, UsersService usersService, IMapper mapper)
        {
            _teamsService = teamsService;
            _usersService = usersService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateTeamModel model)
        {
            try
            {
                var teamId = await _teamsService.CreateTeam(model);
                return Created($"api/teams/{teamId}", new { Id = teamId });
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

        [HttpGet("{teamId}/users")]
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
        public async Task<IActionResult> GetTeam(int teamId)
        {
            var team = await _teamsService.GetTeam(teamId);
            if (team != null)
            {
                return Json(_mapper.Map<TeamViewModel>(team));
            }
            else
            {
                return NotFound(new ErrorResponse
                {
                    Code = ErrorCode.NotFound,
                    Message = "Team with this id is not found"
                });
            }
        }
    }
}
