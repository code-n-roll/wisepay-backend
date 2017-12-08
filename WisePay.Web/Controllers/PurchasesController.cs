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
using WisePay.Web.Purchases;
using WisePay.Web.Teams;
using WisePay.Web.Users;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
    [Authorize]
    public class PurchasesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly UsersService _usersService;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly TeamsService _teamsService;

        public PurchasesController(
            UsersService usersService,
            TeamsService teamsService,
            UserManager<User> userManager,
            ICurrentUserAccessor currentUser,
            IMapper mapper)
        {
            _userManager = userManager;
            _usersService = usersService;
            _teamsService = teamsService;
            _currentUser = currentUser;
            _mapper = mapper;
        }
    }

    [HttpPatch("purchaseId")]
    public async Task<IEnumerable<UserViewModel>> UpdatePurchase(int purchaseId, [FromBody]UpdatePurchaseModel model)
    {

    }
}
