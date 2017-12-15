using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WisePay.DataAccess;
using WisePay.Entities;
using WisePay.Web.Account.Models;
using WisePay.Web.Avatars;
using WisePay.Web.Core.ClientInteraction;
using WisePay.Web.ExternalServices;
using WisePay.Web.Internals;

namespace WisePay.Web.Account
{
    public class AccountService
    {
        private readonly WiseContext _db;
        private readonly BankApi _bankApi;
        private readonly AvatarsService _avatarsService;
        private readonly UserManager<User> _userManager;

        public AccountService(
            WiseContext db,
            BankApi bankApi,
            AvatarsService avatarsService,
            UserManager<User> userManager)
        {
            _db = db;
            _bankApi = bankApi;
            _avatarsService = avatarsService;
            _userManager = userManager;
        }

        public async Task<User> AddBankCard(int userId, BankCardModel cardModel)
        {
            var user = await _db.Users.FindAsync(userId);

            var tokens = await _bankApi.Authenticate(cardModel);
            user.BankActionToken = tokens.AccessToken;
            user.BankIdToken = tokens.IdToken;
            user.CardLastFourDigits = string.Join("", cardModel.CardNumber.Skip(12));

            await _db.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAvatar(int userId, byte[] avatarBytes)
        {
            var user = await _db.Users.FindAsync(userId);

            var newAvatarPath = await _avatarsService.UpdateAvatar(avatarBytes);
            _avatarsService.DeleteAvatar(user.AvatarPath);
            user.AvatarPath = newAvatarPath;

            await _db.SaveChangesAsync();
        }

        public async Task UpdateProfile(int userId, UpdateProfileModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = await _db.Users.FindAsync(userId);

                    user.UserName = model.Username ?? user.UserName;
                    user.Email = model.Email ?? user.Email;

                    var result = await _userManager.UpdateAsync(user);

                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        if (model.Password != model.PasswordConfirmation)
                            throw new ApiException(400, "Passwords don't match", ErrorCode.ValidationError);

                        result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                    }

                    if (model.Avatar != null)
                    {
                        var avatarBytes = Convert.FromBase64String(model.Avatar);

                        var newAvatarPath = await _avatarsService.UpdateAvatar(avatarBytes);
                        _avatarsService.DeleteAvatar(user.AvatarPath);
                        user.AvatarPath = newAvatarPath;
                    }

                    await _userManager.UpdateAsync(user);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
    }
}
