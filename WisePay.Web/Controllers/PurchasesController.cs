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
using WisePay.Web.Purchases.Models;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/users")]
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

        [HttpPatch("{purchaseId}")]
        public async Task<IActionResult> UpdatePurchase(int purchaseId, [FromBody]UpdatePurchaseModel model)
        {
            if (model == null) throw new ApiException(400, "Invalid request body", ErrorCode.InvalidRequestFormat);

            await _purchasesService.UpdatePurchase(purchaseId, model, _currentUser.Id);

            return Ok();
        }

        [HttpGet("my")]
        public async Task<IEnumerable<MyPurchasePreview>> GetMyPurchases()
        {
            var purchases = await _purchasesService.GetUserPurchases(_currentUser.Id);
            return _mapper.Map<IEnumerable<MyPurchasePreview>>(purchases);
        }

        [HttpGet("withme")]
        public async Task<IEnumerable<PurchaseForMe>> GetPurchasesWithMe()
        {
            var purchases = await _purchasesService.GetPurchasesWithUser(_currentUser.Id);
            return _mapper.Map<IEnumerable<PurchaseForMe>>(purchases);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePurchase([FromBody]CreatePurchaseModel model)
        {
            if (model == null) throw new ApiException(400, "Invalid request body", ErrorCode.InvalidRequestFormat);

            var purchaseId = await _purchasesService.CreatePurchase(model, _currentUser.Id);

            return Created($"/api/purchases/{purchaseId}", new { Id = purchaseId });
        }

        [HttpGet("purchaseId")]
        public async Task<MyPurchase> Get(int purchaseId)
        {
            var purchase = await _purchasesService.GetPurchase(purchaseId);
            if (purchase == null)
                throw new ApiException(404, "Purchase not found", ErrorCode.NotFound);

            if (purchase.CreatorId != _currentUser.Id)
                throw new ApiException(401, "Access denied", ErrorCode.AuthError);

            return _mapper.Map<MyPurchase>(purchase);
        }
    }
}
