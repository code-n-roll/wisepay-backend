using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WisePay.DataAccess;
using WisePay.Entities;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Internals;
using WisePay.Web.Purchases.Models;

namespace WisePay.Web.Purchases
{
    public class PurchasesService
    {
        private WiseContext _db;

        public PurchasesService(WiseContext db)
        {
            _db = db;
        }

        public async Task UpdatePurchase(int purchaseId, UpdatePurchaseModel model, int currentUserId)
        {
            var purchase = await _db.Purchases.FindAsync(purchaseId);

            if (purchase.CreatorId != currentUserId)
                throw new ApiException(401, "Access denied", ErrorCode.AuthError);

            purchase.Name = string.IsNullOrWhiteSpace(model.Name) ? purchase.Name : model.Name;

            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<Purchase>> GetUserPurchases(int userId)
        {
            return await _db.Purchases
                .Where(p => p.CreatorId == userId)
                .ToListAsync();
        }

        public async Task<Purchase> GetPurchase(int purchaseId)
        {
            return await _db.UserPurchases
                .Include(up => up.Purchase)
                .Include(up => up.User)
                .Where(up => up.PurchaseId == purchaseId)
                .Select(up => up.Purchase)
                .FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<UserPurchase>> GetPurchasesWithUser(int userId)
        {
            return await _db.UserPurchases
                .Include(up => up.Purchase)
                .ThenInclude(p => p.Creator)
                .Where(up => up.UserId == userId)
                .ToListAsync();
        }

        public async Task<int> CreatePurchase(CreatePurchaseModel model, int currentUserId)
        {
            var purchase = new Purchase
            {
                CreatorId = currentUserId,
                IsPayedOff = false,
                Name = model.Name,
                TotalSum = model.TotalAmount,
                CreatedAt = DateTime.Now
            };

            _db.Purchases.Add(purchase);
            await _db.SaveChangesAsync();

            var userPurchases = model.Users.Select(u => new UserPurchase
            {
                PurchaseId = purchase.Id,
                UserId = u.UserId,
                Sum = u.Amount,
                IsPayedOff = false
            });

            _db.UserPurchases.AddRange(userPurchases);
            await _db.SaveChangesAsync();
            return purchase.Id;
        }
    }
}
