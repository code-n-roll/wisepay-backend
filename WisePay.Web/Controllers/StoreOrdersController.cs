using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WisePay.Entities;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.ExternalServices.Crawler.Responses;
using WisePay.Web.Internals;
using WisePay.Web.Purchases;
using WisePay.Web.Purchases.Models;

namespace WisePay.Web.Controllers
{
    [Produces("application/json")]
    [Route("api/orders")]
    [Authorize]
    public class StoreOrdersController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly StoreOrdersService _storeOrdersService;
        private readonly ICurrentUserAccessor _currentUser;

        public StoreOrdersController(
            StoreOrdersService storeOrdersService,
            UserManager<User> userManager,
            ICurrentUserAccessor currentUser,
            IMapper mapper)
        {
            _userManager = userManager;
            _storeOrdersService = storeOrdersService;
            _currentUser = currentUser;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrders([FromBody] CreateStoreOrdersModel model)
        {
            if (model == null)
                throw new ApiException(400, "Invalid request body", ErrorCode.InvalidRequestFormat);

            var currentUser = new UserStoreOrderModel
            {
                UserId = _currentUser.Id
            };
            var purchase = await _storeOrdersService.CreatePurchase(model, currentUser);
            return Created($"/api/purchases/{purchase.Id}", _mapper.Map<MyPurchase>(purchase));
        }

        [HttpPatch("{purchaseId}")]
        public async Task<IActionResult> UpdateOrder(int purchaseId, [FromBody] SubmitOrderItemsModel model)
        {
            if (model == null)
                throw new ApiException(400, "Invalid request body", ErrorCode.InvalidRequestFormat);

            await _storeOrdersService.UpdateOrder(purchaseId, model, _currentUser.Id);

            return Ok();
        }

        [HttpPost("{purchaseId}/submit")]
        public async Task<IActionResult> SubmitOrder(int purchaseId)
        {
            await _storeOrdersService.SubmitOrder(purchaseId, _currentUser.Id);
            return Ok();
        }

        [HttpGet("stores")]
        public async Task<List<StoreResponse>> GetStores()
        {
            var stores = await _storeOrdersService.GetStores();
            return _mapper.Map<List<StoreResponse>>(stores);
        }

        [HttpGet("stores/{storeId}")]
        public async Task<StoreResponse> GetStoreContent(string storeId)
        {
            var categories = await _storeOrdersService.GetStoreContent(storeId);
            return _mapper.Map<StoreResponse>(categories);
        }

        [HttpGet("items")]
        public async Task<List<ItemResponse>> GetItems([FromQuery] string categoryId, [FromQuery] string ids)
        {
            var itemsIds = ids?.Split(",").ToList();
            var items = await _storeOrdersService.GetItems(categoryId, itemsIds);
            return _mapper.Map<List<ItemResponse>>(items);
        }
    }
}
