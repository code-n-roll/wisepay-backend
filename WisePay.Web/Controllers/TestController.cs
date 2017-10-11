using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WisePay.Entities;

namespace WisePay.Web.Controllers
{
    [Route("api/Test")]
    public class TestController : Controller
    {
        private UserManager<User> _userManager;

        public TestController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet]
        public async Task<IEnumerable<string>> GetValues()
        {
            var user = await _userManager.GetUserAsync(User);

            return new List<string>
            {
                "123",
                "456"
            };
        }
    }
}