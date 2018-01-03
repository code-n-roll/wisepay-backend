using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WisePay.Web.Debts;
using WisePay.Web.Debts.Models;
using WisePay.Web.Internals;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/debts")]
    [Authorize]
    public class DebtsController : Controller
    {
        private readonly DebtsService _debtsService;
        private readonly ICurrentUserAccessor _currentUser;
        private readonly IMapper _mapper;

        public DebtsController(
            DebtsService debtsService,
            ICurrentUserAccessor currentUser,
            IMapper mapper)
        {
            _debtsService = debtsService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<DebtsStats> GetDebtsStats()
        {
            var (usersWithDebts, total) = await _debtsService.CalculateDebtsStats(_currentUser.Id);

            var response = new DebtsStats
            {
                TotalStats = total,
                Users = _mapper.Map<IEnumerable<UserWithDebt>>(
                    usersWithDebts.OrderByDescending(u => Math.Abs(u.Debt)))
            };

            return response;
        }
    }
}
