using System;
using System.Collections.Generic;
using System.IO;
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
using WisePay.Web.Core.Helpers;
using WisePay.Web.ExternalServices;
using WisePay.Web.ExternalServices.Bank;
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

        public async Task<User> RemoveBankCard(int userId)
        {
            var user = await _db.Users.FindAsync(userId);

            user.BankActionToken = null;
            user.BankIdToken = null;
            user.CardLastFourDigits = null;

            await _db.SaveChangesAsync();
            return user;
        }

        public async Task UpdateAvatar(int userId, byte[] avatarBytes, string extension)
        {
            var user = await _db.Users.FindAsync(userId);

            var newAvatarPath = await _avatarsService.UpdateAvatar(avatarBytes, extension);
            _avatarsService.DeleteAvatar(user.AvatarPath);
            user.AvatarPath = newAvatarPath;

            await _db.SaveChangesAsync();
        }

        public async Task<User> UpdateProfile(int userId, UpdateProfileModel model)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    var user = await _db.Users.FindAsync(userId);

                    user.UserName = model.Username ?? user.UserName;
                    user.Email = model.Email ?? user.Email;

                    var result = await _userManager.UpdateAsync(user);
                    ErrorResultsHandler.ThrowIfIdentityError(result);

                    if (!string.IsNullOrEmpty(model.NewPassword))
                    {
                        if (model.Password != model.PasswordConfirmation)
                            throw new ApiException(400, "Passwords don't match", ErrorCode.ValidationError);

                        result = await _userManager.ChangePasswordAsync(user, model.Password, model.NewPassword);
                        ErrorResultsHandler.ThrowIfIdentityError(result);
                    }

                    if (model.Avatar != null)
                    {
                        byte[] avatarBytes = null;

                        using (var memoryStream = new MemoryStream())
                        {
                            await model.Avatar.CopyToAsync(memoryStream);
                            avatarBytes = memoryStream.ToArray();
                        }

                        var fileExtension = Path.GetExtension(model.Avatar.FileName);


                        var newAvatarPath =
                            await _avatarsService.UpdateAvatar(avatarBytes, fileExtension);

                        if (user.AvatarPath != null)
                        {
                            _avatarsService.DeleteAvatar(user.AvatarPath);
                        }

                        user.AvatarPath = newAvatarPath;
                    }

                    await _userManager.UpdateAsync(user);
                    transaction.Commit();

                    return user;
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
