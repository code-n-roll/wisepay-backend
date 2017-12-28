using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/debts")]
    [Authorize]
    public class DebtsController : Controller
    {
        public DebtsController()
        {

        }

        [HttpGet]
        public async Task GetDebtsStats()
        {
            throw new NotImplementedException();
        }
    }
}
