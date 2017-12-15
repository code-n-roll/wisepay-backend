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
using WisePay.Web.Purchases.Models;
using WisePay.Web.Teams;
using WisePay.Web.Teams.Models;
using WisePay.Web.Users;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/purchases")]
    [Authorize]
    public class PurchasesController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly PurchasesService _purchasesService;
        private readonly ICurrentUserAccessor _currentUser;

        public PurchasesController(
            PurchasesService purchasesService,
            UserManager<User> userManager,
            ICurrentUserAccessor currentUser,
            IMapper mapper)
        {
            _userManager = userManager;
            _purchasesService = purchasesService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetPurchases()
        {
            var (myPurchases, purchasesWithMe) = await _purchasesService.GetUserPurchases(_currentUser.Id);

            IEnumerable<PurchaseViewModel> myPurchasesViewModel =
                _mapper.Map<IEnumerable<MyPurchase>>(myPurchases);
            IEnumerable<PurchaseViewModel> purchasesWithMeViewModel =
                _mapper.Map<IEnumerable<PurchaseWithMe>>(purchasesWithMe);

            return Json(myPurchasesViewModel.Concat(purchasesWithMeViewModel)
                .OrderByDescending(p => p.CreatedAt));
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchase([FromBody]CreatePurchaseModel model)
        {
            if (model == null) throw new ApiException(400, "Invalid request body", ErrorCode.InvalidRequestFormat);

            var purchaseId = await _purchasesService.CreatePurchase(model, _currentUser.Id);

            return Created($"/api/purchases/{purchaseId}", new { Id = purchaseId });
        }

        [HttpPatch("{purchaseId}")]
        public async Task<IActionResult> UpdatePurchase(int purchaseId, [FromBody]UpdatePurchaseModel model)
        {
            if (model == null) throw new ApiException(400, "Invalid request body", ErrorCode.InvalidRequestFormat);

            await _purchasesService.UpdatePurchase(purchaseId, model, _currentUser.Id);

            return Ok();
        }

        [HttpPost("{purchaseId}/pay")]
        public async Task<IActionResult> Pay(int purchaseId, [FromBody]PayModel model)
        {
            await _purchasesService.PayForPurchase(purchaseId, _currentUser.Id, model.Sum);
            return Ok();
        }

        [HttpPost("{purchaseId}/decline")]
        public async Task<IActionResult> Decline(int purchaseId)
        {
            await _purchasesService.DeclinePurchase(purchaseId, _currentUser.Id);
            return Ok();
        }

        [HttpPost("sendmoney")]
        public async Task<IActionResult> SendMoney([FromBody]SendMoneyModel model)
        {
            await _purchasesService.SendMoney(_currentUser.Id, model.UserId, model.Sum);
            return Ok();
        }
    }
}
