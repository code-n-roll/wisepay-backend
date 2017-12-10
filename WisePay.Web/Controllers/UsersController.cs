using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WisePay.Entities;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Internals;
using WisePay.Web.Purchases;
using WisePay.Web.Teams;
using WisePay.Web.Teams.Models;
using WisePay.Web.Users;
using WisePay.Web.Users.Models;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    [Authorize]
    public class UsersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly UsersService _usersService;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly TeamsService _teamsService;
        private readonly PurchasesService _purchasesService;

        public UsersController(
            UsersService usersService,
            TeamsService teamsService,
            PurchasesService purchasesService,
            UserManager<User> userManager,
            ICurrentUserAccessor currentUser,
            IMapper mapper)
        {
            _userManager = userManager;
            _usersService = usersService;
            _teamsService = teamsService;
            _purchasesService = purchasesService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<UserViewModel>> GetAll(string query)
        {
            var users = await _usersService.GetAllByQuery(query);
            return _mapper.Map<IEnumerable<UserViewModel>>(users);
        }

        [HttpGet("{id}")]
        public async Task<UserViewModel> Get(int id)
        {
            var user = await _usersService.GetById(id);

            if (user == null) throw new ApiException(404, "User not found", ErrorCode.NotFound);

            return _mapper.Map<UserViewModel>(user);
        }

        [HttpGet("me")]
        public async Task<CurrentUserViewModel> GetMe()
        {
            var me = await _userManager.GetUserAsync(User);
            return _mapper.Map<CurrentUserViewModel>(me);
        }

        [HttpGet("me/teams")]
        public async Task<IEnumerable<TeamPreview>> GetMyTeams()
        {
            var teams = await _teamsService.GetUserTeams(_currentUser.Id);
            return _mapper.Map<IEnumerable<TeamPreview>>(teams);
        }
    }
}
