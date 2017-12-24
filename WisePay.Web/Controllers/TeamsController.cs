using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WisePay.Entities;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Internals;
using WisePay.Web.Teams;
using WisePay.Web.Teams.Models;
using WisePay.Web.Users;
using WisePay.Web.Users.Models;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/Teams")]
    [Authorize]
    public class TeamsController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly TeamsService _teamsService;
        private readonly UsersService _usersService;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IMapper _mapper;

        public TeamsController(
            TeamsService teamsService,
            UsersService usersService,
            UserManager<User> userManager,
            ICurrentUserAccessor currentUser,
            IMapper mapper)
        {
            _userManager = userManager;
            _teamsService = teamsService;
            _usersService = usersService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateTeamModel model)
        {
            if (model.UserIds.Contains(_currentUser.Id))
            {
                throw new ApiException(400, "Invalid user id list", ErrorCode.InvalidRequestFormat);
            }

            var teamId = await _teamsService.CreateTeam(model, _currentUser.Id);
            return Created($"api/teams/{teamId}", new { Id = teamId });
        }

        [HttpGet("{teamId}/users")]
        public async Task<IEnumerable<UserViewModel>> GetUsersInTeam(int teamId)
        {
            var users = await _usersService.GetUsersInTeam(teamId);

            if (!users.Any(u => u.Id == _currentUser.Id))
            {
                throw new ApiException(401, "You are not authorized for this resource", ErrorCode.AuthError);
            }

            return _mapper.Map<IEnumerable<UserViewModel>>(users);
        }

        [HttpGet("{teamId}")]
        public async Task<TeamViewModel> GetTeam(int teamId)
        {
            var team = await _teamsService.GetTeam(teamId);

            if (team == null)
            {
                throw new ApiException(404, "Team with this id is not found", ErrorCode.NotFound);
            }

            if (!team.UserTeams.Select(ut => ut.User).Any(u => u.Id == _currentUser.Id))
            {
                throw new ApiException(401, "You are not authorized for this resource", ErrorCode.AuthError);
            }

            return _mapper.Map<TeamViewModel>(team);
        }

        [HttpPatch("{teamId}")]
        public async Task<IActionResult> UpdateTeam(int teamId, [FromBody]UpdateTeamModel model)
        {
            if (model == null) throw new ApiException(400, "Invalid request body", ErrorCode.InvalidRequestFormat);

            await _teamsService.UpdateTeam(teamId, model, _currentUser.Id);

            return Ok();
        }

        [HttpPut("{teamId}/users")]
        public async Task<IActionResult> UpdateTeamUsers(int teamId, [FromBody]UpdateTeamUsersModel model)
        {
            if (model == null || model.UserIds == null)
                throw new ApiException(400, "Invalid request body", ErrorCode.InvalidRequestFormat);

            await _teamsService.UpdateTeamUsers(teamId, model, _currentUser.Id);

            return Ok();
        }
    }
}
