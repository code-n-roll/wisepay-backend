using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WisePay.Entities;
using WisePay.Web.Account;
using WisePay.Web.Account.Models;
using WisePay.Web.Auth;
using WisePay.Web.Auth.Models;
using WisePay.Web.Avatars;
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
        private readonly ICurrentUserAccessor _currentUser;
        private readonly AccountService _accountService;
        private readonly IMapper _mapper;

        public AccountController(
            UserManager<User> userManager,
            AccountService accountService,
            IMapper mapper,
            ICurrentUserAccessor currentUser)
        {
            _userManager = userManager;
            _currentUser = currentUser;
            _accountService = accountService;
            _mapper = mapper;
        }

        [HttpPost("addCard")]
        public async Task<CurrentUserViewModel> AddBankCard([FromBody]BankCardModel cardModel)
        {
            var user = await _accountService.AddBankCard(_currentUser.Id, cardModel);
            return _mapper.Map<CurrentUserViewModel>(user);
        }

        [HttpPost("updateAvatar")]
        public async Task<IActionResult> UpdateAvatar(IFormFile avatarData)
        {
            byte[] avatarBytes = null;
            using (var memoryStream = new MemoryStream())
            {
                await avatarData.CopyToAsync(memoryStream);
                avatarBytes = memoryStream.ToArray();
            }

            await _accountService.UpdateAvatar(_currentUser.Id, avatarBytes);
            return Ok();
        }

        [HttpPost("updateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody]UpdateProfileModel model)
        {
            await _accountService.UpdateProfile(_currentUser.Id, model);
            return Ok();
        }

        [HttpGet]
        public async Task<ProfileViewModel> GetProfileData()
        {
            var user = await _userManager.FindByIdAsync(_currentUser.Id.ToString());

            return _mapper.Map<ProfileViewModel>(user);
        }
    }
}
