using ClothingStore.Entities;
using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Account;
using E_Commerce.Entities.Model.authonution;
using E_Commerce.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System.Drawing.Text;

namespace E_Commerce.services.AccountManager
{
    public class AccountManagerServices:IAccountManagerServices
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;
        public AccountManagerServices(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<Helpers.JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }
        #region authentication with email confirmation and password reset
   

        public async Task<string> ForgotPasswordAsync(ForgotPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return "User not found!";
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            return token;
        }

        public async Task<AuthModel> ResetPasswordAsync(ResetPassword model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                authModel.Message = "User not found!";
                return authModel;
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                authModel.Message = "Password reset failed!";
                return authModel;
            }
            authModel.IsAuthenticated = true;
            authModel.Message = "Password reset successfully!";
            return authModel;

        }

        public async Task<Profile> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return null;
            }
            var profile = new Profile
            {
                //  UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return profile;
        }

        public async Task<AuthModel> UpdateProfileAsync(string userId, UpdateProfile model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                authModel.Message = "User not found!";
                return authModel;
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                authModel.Message = "Profile update failed!";
                return authModel;
            }
            authModel.IsAuthenticated = true;
            authModel.Message = "Profile updated successfully!";
            return authModel;
        }

        #endregion
    }
}

