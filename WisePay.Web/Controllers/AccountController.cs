using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WisePay.Entities;
using WisePay.Web.Account;
using WisePay.Web.Account.Models;
using WisePay.Web.Auth;
using WisePay.Web.Auth.Models;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.ExternalServices;
using WisePay.Web.Internals;
using WisePay.Web.Users.Models;

namespace WisePay.Web.Controllers
{
    [Route("api/account")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AuthTokenService _tokenService;
        private readonly BankApi _bankApi;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly AccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<User> userManager,
            AuthTokenService tokenService,
            BankApi bankApi,
            AccountService accountService,
            IMapper mapper,
            ICurrentUserAccessor currentUser)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _bankApi = bankApi;
            _currentUser = currentUser;
            _accountService = accountService;
            _mapper = mapper;
        }
        
        [HttpPost("addcard")]
        public async Task<CurrentUserViewModel> AddBankCard([FromBody]BankCardModel cardModel)
        {
            var user = await _accountService.AddBankCard(_currentUser.Id, cardModel);
            return _mapper.Map<CurrentUserViewModel>(user);
        }
    }
}
