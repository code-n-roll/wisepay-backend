using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WisePay.DataAccess;
using WisePay.Entities;
using WisePay.Web.Account.Models;
using WisePay.Web.ExternalServices;

namespace WisePay.Web.Account
{
    public class AccountService
    {
        private readonly WiseContext _db;
        private readonly BankApi _bankApi;

        public AccountService(WiseContext db, BankApi bankApi)
        {
            _db = db;
            _bankApi = bankApi;
        }

        public async Task<User> AddBankCard(int userId, BankCardModel cardModel)
        {
            var user = await _db.Users.FindAsync(userId);

            var tokens = await _bankApi.Authenticate(cardModel);
            user.BankActionToken = tokens.AccessToken;
            user.BankIdToken = tokens.IdToken;
            user.CardLastFourDigits = cardModel.CardNumber.Skip(12).ToString();

            await _db.SaveChangesAsync();
            return user;
        }
    }
}
