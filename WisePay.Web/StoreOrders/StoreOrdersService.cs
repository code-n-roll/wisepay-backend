using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WisePay.DataAccess;
using WisePay.Entities;
using WisePay.Web.ExternalServices.Crawler;
using WisePay.Web.ExternalServices.Crawler.Responses;
using WisePay.Web.Purchases.Models;

namespace WisePay.Web.Purchases
{
    public class StoreOrdersService
    {
        private readonly WiseContext _db;
        private readonly CrawlerApi _crawlerApi;

        public StoreOrdersService(WiseContext db, CrawlerApi crawlerApi)
        {
            _db = db;
            _crawlerApi = crawlerApi;
        }

        public async Task<Purchase> CreatePurchase(CreateStoreOrdersModel model, UserStoreOrderModel currentUser)
        {
            var storeOrder = new StoreOrder
            {
                StoreId = model.StoreId,
                IsSubmitted = false
            };

            var purchase = new Purchase
            {
                Type = PurchaseType.Store,
                CreatorId = currentUser.UserId,
                IsPayedOff = false,
                Name = model.Name,
                CreatedAt = DateTime.Now,
                StoreOrder = storeOrder
            };

            _db.Purchases.Add(purchase);
            await _db.SaveChangesAsync();

            model.Users = model.Users.Concat(new[] {currentUser});
            var userPurchases = model.Users.Select(u => new UserPurchase
            {
                PurchaseId = purchase.Id,
                UserId = u.UserId,
                Status = PurchaseStatus.New
            });


            _db.UserPurchases.AddRange(userPurchases);
            await _db.SaveChangesAsync();

            purchase.UserPurchases = await _db.UserPurchases
                .Include(up => up.User)
                .Where(up => up.PurchaseId == purchase.Id)
                .ToListAsync();
            return purchase;
        }

        public async Task UpdateOrder(int purchaseId, SubmitOrderItemsModel model, int currentUserId)
        {
            var purchase = await _db.Purchases.FindAsync(purchaseId);
            var userPurchase = purchase.UserPurchases.First(up => up.UserId == currentUserId);

            var items = model.Items.Select(u => new UserPurchaseItem
            {
                ItemId = u.ItemId,
                Number = u.Number,
                Price = u.Price
            });
            _db.UserPurchaseItems.AddRange(items);
            await _db.SaveChangesAsync();

            userPurchase.Items.Clear();

            userPurchase.Items = await _db.UserPurchaseItems
                .Include(up => up.UserPurchase)
                .Where(up => up.UserPurchase.UserId == userPurchase.UserId)
                .Where(up => up.UserPurchase.PurchaseId == userPurchase.PurchaseId)
                .ToListAsync();

            await _db.SaveChangesAsync();
        }

        public async Task<List<StoreResponse>> GetStores()
        {
            return await _crawlerApi.GetStores();
        }

        public async Task<List<CategoryResponse>> GetCategories(string storeId)
        {
            return await _crawlerApi.GetCategories(storeId);
        }

        public async Task<List<ItemResponse>> GetItems(string categoryId, IEnumerable<string> itemIds)
        {
            return await _crawlerApi.GetItems(categoryId, itemIds);
        }
    }
}
