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
using WisePay.Web.Users;

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
        private readonly IMapper _mapper;

        public TeamsController(
            TeamsService teamsService,
            UsersService usersService,
            UserManager<User> userManager,
            IMapper mapper)
        {
            _userManager = userManager;
            _teamsService = teamsService;
            _usersService = usersService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody]CreateTeamModel model)
        {
            var me = await _userManager.GetUserAsync(User);
            var teamId = await _teamsService.CreateTeam(model, me.Id);
            return Created($"api/teams/{teamId}", new { Id = teamId });
        }

        [HttpGet("{teamId}/users")]
        public async Task<IEnumerable<UserViewModel>> GetUsersInTeam(int teamId)
        {
            var me = await _userManager.GetUserAsync(User);
            var users = await _usersService.GetUsersInTeam(teamId);

            if (!users.Any(u => u.Id == me.Id))
            {
                throw new ApiException(401, "You are not authorized for this resource", ErrorCode.AuthError);
            }

            return _mapper.Map<IEnumerable<UserViewModel>>(users);
        }

        [HttpGet("{id}")]
        public async Task<TeamViewModel> GetTeam(int teamId)
        {
            var team = await _teamsService.GetTeam(teamId);

            if (team == null)
            {
                throw new ApiException(404, "Team with this id is not found", ErrorCode.NotFound);
            }

            return _mapper.Map<TeamViewModel>(team);
        }


    }
}
