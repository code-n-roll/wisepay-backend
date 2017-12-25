using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WisePay.Entities;
using WisePay.Web.Avatars;
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
        private readonly AvatarsService _avatarsService;
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
            AvatarsService avatarsService,
            IMapper mapper)
        {
            _userManager = userManager;
            _usersService = usersService;
            _teamsService = teamsService;
            _purchasesService = purchasesService;
            _currentUser = currentUser;
            _mapper = mapper;
            _avatarsService = avatarsService;
        }

        [HttpGet]
        public async Task<IEnumerable<UserViewModel>> GetAll(string query)
        {
            var users = await _usersService.GetAllByQuery(query);
            users = users.Where(u => u.Id != _currentUser.Id);

            var userViewModels = users.Select(u =>
            {
                var userViewModel = _mapper.Map<UserViewModel>(u);
                userViewModel.AvatarUrl = _avatarsService.GetFullAvatarUrl(u.AvatarPath);
                return userViewModel;
            });
            return userViewModels;
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
    }
}
