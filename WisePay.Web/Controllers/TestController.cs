using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace WisePay.Web.Controllers
{
    [Route("api/Test")]
    public class TestController : Controller
    {
        [Authorize]
        [HttpGet]
        public IEnumerable<string> GetValues()
        {
            return new List<string>
            {
                "123",
                "456"
            };
        }
    }
}