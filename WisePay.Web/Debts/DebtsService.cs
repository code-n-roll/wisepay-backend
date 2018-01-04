using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WisePay.DataAccess;
using WisePay.Entities;
using WisePay.Web.Avatars;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.Debts.Models;
using WisePay.Web.Internals;
using WisePay.Web.Purchases;

namespace WisePay.Web.Debts
{
    public class DebtsService
    {
        private readonly WiseContext _db;
        private readonly PurchasesService _purchasesService;
        private readonly AvatarsService _avatarsService;

        public DebtsService(
            WiseContext db,
            PurchasesService purchasesService,
            AvatarsService avatarsService)
        {
            _db = db;
            _purchasesService = purchasesService;
            _avatarsService = avatarsService;
        }

        public async Task<(IList<UserWithDebtRaw> users, TotalStats total)>
            CalculateDebtsStats(int userId)
        {
            var currentUser = await _db.Users.FindAsync(userId);
            if (currentUser == null)
                throw new ApiException(404, "User not found", ErrorCode.NotFound);

            var myPurchasesGroups = await _db.UserPurchases
                .Include(up => up.User)
                .Where(up => up.Status == PurchaseStatus.New)
                .Where(up => up.Purchase.CreatorId == userId &&
                    up.UserId != userId)
                .GroupBy(up => up.UserId)
                .ToListAsync();

            var purchasesWithMeGroups = await _db.UserPurchases
                .Include(up => up.Purchase)
                    .ThenInclude(p => p.Creator)
                .Where(up => up.Status == PurchaseStatus.New)
                .Where(up => up.UserId == userId &&
                    up.Purchase.CreatorId != userId)
                .GroupBy(up => up.Purchase.CreatorId)
                .ToListAsync();

            // TODO How total stats should be calculated??
            var totalStats = new TotalStats();
            var users = new Dictionary<int, UserWithDebtRaw>();

            foreach (var myPurchases in myPurchasesGroups)
            {
                var userPurchase = myPurchases.First();
                var userTotalDebt = myPurchases
                    .Aggregate<UserPurchase, decimal>(0, (sum, u) => sum + (u.Sum ?? 0));

                if (userTotalDebt != 0)
                {
                    users.Add(myPurchases.Key, new UserWithDebtRaw
                    {
                        User = userPurchase.User,
                        Debt = userTotalDebt
                    });

                    totalStats.DebtToMe += userTotalDebt;
                }
            }

            foreach (var purchasesWithMe in purchasesWithMeGroups)
            {
                var userPurchase = purchasesWithMe.First();
                var  myTotalDebt = purchasesWithMe
                    .Aggregate<UserPurchase, decimal>(0, (sum, u) => sum + (u.Sum ?? 0));

                if (myTotalDebt != 0)
                {
                    if (users.ContainsKey(purchasesWithMe.Key))
                    {
                        users[purchasesWithMe.Key].Debt -= myTotalDebt;
                    }
                    else
                    {
                        users.Add(purchasesWithMe.Key, new UserWithDebtRaw
                        {
                            User = userPurchase.Purchase.Creator,
                            Debt = -myTotalDebt
                        });
                    }

                    totalStats.MyDebt += myTotalDebt;
                }
            }

            var rawUsers = users.Values.ToList();
            var allUsers = await _db.Users.Where(u => u.Id != currentUser.Id).ToListAsync();
            var includedUsers = users.Values.Select(raw => raw.User.Id).ToHashSet();
            rawUsers.AddRange(from user in allUsers
                where !includedUsers.Contains(user.Id)
                select new UserWithDebtRaw
                {
                    User = user,
                    Debt = 0
                });


            return (rawUsers, totalStats);
        }
    }
}
