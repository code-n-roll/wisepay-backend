using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WisePay.DataAccess;
using WisePay.Web.Avatars;
using WisePay.Web.Core.ClientInteraction;
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

        public async Task CalculateDebtsStats(int userId)
        {
            throw new NotImplementedException();
            //var user = await _db.Users.FindAsync(userId);
            //if (user == null)
            //    throw new ApiException(404, "User not found", ErrorCode.NotFound);

            //_db.Purchases
            //    .Where(p => p.CreatorId == userId)
            //    .Select(p => p.UserPurchases.GroupBy())

            //var (myPurchases, purchasesWithMe) = await _purchasesService.GetUserPurchases(userId);

        }
    }
}
