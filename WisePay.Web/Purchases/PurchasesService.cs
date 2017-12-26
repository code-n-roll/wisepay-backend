using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WisePay.DataAccess;
using WisePay.Entities;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.ExternalServices;
using WisePay.Web.ExternalServices.Bank;
using WisePay.Web.Internals;
using WisePay.Web.Purchases.Models;

namespace WisePay.Web.Purchases
{
    public class PurchasesService
    {
        private readonly WiseContext _db;
        private readonly BankApi _bankApi;

        public PurchasesService(WiseContext db, BankApi bankApi)
        {
            _db = db;
            _bankApi = bankApi;
        }

        public async Task UpdatePurchase(int purchaseId, UpdatePurchaseModel model, int currentUserId)
        {
            var purchase = await _db.Purchases.FindAsync(purchaseId);

            if (purchase.CreatorId != currentUserId)
                throw new ApiException(401, "Access denied", ErrorCode.AuthError);

            purchase.Name = string.IsNullOrWhiteSpace(model.Name) ? purchase.Name : model.Name;

            await _db.SaveChangesAsync();
        }

        public async Task<(IEnumerable<Purchase>, IEnumerable<UserPurchase>)> GetUserPurchases(int userId)
        {
            var myPurchases = await _db.Purchases
                .Where(p => p.CreatorId == userId)
                .Include(up => up.StoreOrder)
                .Include(up => up.UserPurchases)
                .ThenInclude(p => p.User)
                .Include(up => up.UserPurchases)
                .ThenInclude(p => p.Items)
                .ToListAsync();

            var purchasesWithMe = await _db.UserPurchases
                .Include(up => up.Purchase.Creator)
                .Include(up => up.Purchase.StoreOrder)
                .Include(up => up.Items)
                .Where(up => up.UserId == userId)
                .Where(up => up.Purchase.CreatorId != userId)
                .ToListAsync();

            return (myPurchases, purchasesWithMe);
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

        public async Task<Purchase> CreatePurchase(CreatePurchaseModel model, int currentUserId)
        {
            var purchase = new Purchase
            {
                Type = PurchaseType.Custom,
                CreatorId = currentUserId,
                IsPayedOff = false,
                Name = model.Name,
                TotalSum = model.TotalSum,
                CreatedAt = DateTime.Now
            };

            _db.Purchases.Add(purchase);
            await _db.SaveChangesAsync();

            var userPurchases = model.Users.Select(u => new UserPurchase
            {
                PurchaseId = purchase.Id,
                UserId = u.UserId,
                Sum = u.Sum,
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

        public async Task PayForPurchase(int purchaseId, int currentUserId, decimal? sum)
        {
            var userPurchase = await GetUserPurchase(purchaseId, currentUserId);

            if (userPurchase == null)
                throw new ApiException(404, "Not found", ErrorCode.NotFound);

            if (userPurchase.Status != PurchaseStatus.New)
                throw new ApiException(400, "Already payed");

            var sumToPay = userPurchase.Sum
                ?? sum
                ?? throw new ApiException(400, "This purchase required user-entered sum",
                    ErrorCode.InvalidRequestFormat);

            var recipientIdToken = userPurchase.Purchase.Creator.BankIdToken;
            var myActionToken = userPurchase.User.BankActionToken;

            if (recipientIdToken == null)
                throw new ApiException(400, "Recipient didn't bind bank card");

            if (myActionToken == null)
                throw new ApiException(400, "You don't have any bank card");

            await _bankApi.SendMoney(myActionToken, recipientIdToken, sumToPay);

            _db.PaymentHistory.Add(new PaymentHistoryItem
            {
                PurchaseId = userPurchase.PurchaseId,
                UserId = userPurchase.UserId,
                Sum = sumToPay,
                Timestamp = DateTime.Now
            });

            userPurchase.Status = PurchaseStatus.Payed;
            await _db.SaveChangesAsync();

            await UpdatePurchaseInfo(userPurchase.Purchase.Id);
        }

        public async Task DeclinePurchase(int purchaseId, int currentUserId)
        {
            var userPurchase = await GetUserPurchase(purchaseId, currentUserId);

            if (userPurchase == null)
                throw new ApiException(404, "Not found", ErrorCode.NotFound);

            if (userPurchase.Status != PurchaseStatus.New)
                throw new ApiException(400, "Already payed");

            userPurchase.Status = PurchaseStatus.Declined;
            if (userPurchase.Sum != null)
            {
                userPurchase.Purchase.TotalSum -= userPurchase.Sum.Value;
            }
            await _db.SaveChangesAsync();

            await UpdatePurchaseInfo(userPurchase.Purchase.Id);
        }

        public async Task SendMoney(int currentUserId, int recipientId, decimal sum)
        {
            var me = await _db.Users.FindAsync(currentUserId);
            var recipient = await _db.Users.FindAsync(recipientId);

            if (recipient == null)
                throw new ApiException(404, "User not found", ErrorCode.NotFound);

            if (recipient.BankIdToken == null)
                throw new ApiException(400, "Recipient haven't added any bank card");

            if (me.BankActionToken == null)
                throw new ApiException(400, "You don't have any bank card");

            await _bankApi.SendMoney(me.BankActionToken, recipient.BankIdToken, sum);
        }

        private Task<UserPurchase> GetUserPurchase(int purchaseId, int currentUserId)
        {
            return _db.UserPurchases
                .Include(up => up.User)
                .Include(up => up.Purchase)
                .ThenInclude(p => p.Creator)
                .Where(up => up.PurchaseId == purchaseId && up.UserId == currentUserId)
                .FirstOrDefaultAsync();
        }

        private async Task UpdatePurchaseInfo(int purchaseId)
        {
            var purchase = await _db.Purchases
                .Include(p => p.UserPurchases)
                .Where(p => p.Id == purchaseId)
                .FirstOrDefaultAsync();

            if (purchase.UserPurchases.All(up => up.Status != PurchaseStatus.New))
            {
                purchase.IsPayedOff = true;
                await _db.SaveChangesAsync();
            }
        }
    }
}
