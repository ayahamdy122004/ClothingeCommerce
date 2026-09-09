using ClothingStore.Entities;
using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Account;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model.authonution;
using E_Commerce.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using NuGet.Common;
using System.Drawing.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace E_Commerce.services.AccountManager
{
    public class AccountManagerServices:IAccountManagerServices
    {
        #region
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
        #endregion
        #region authentication with email confirmation and password reset


        public async Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return ApiResponse<string>.FailureResponse(
                    message: "User with this email does not exist.",
                    statusCode: 404,
                    errors: new List<string> { "User not found." }
                );
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            return ApiResponse<string>.SuccessResponse(
                data: token,
                message: "Password reset token generated successfully.",
                statusCode: 200
            );
        }

       public async Task<ApiResponse<AuthModel>> ResetPasswordAsync(ResetPassword model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user==null)
            {
                return ApiResponse<AuthModel>.FailureResponse(
                    message: "Invalid request data.",
                    statusCode: 400,
                    errors: new List<string> { "Email, token, and new password are required." }
                );
            }
 var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (!result.Succeeded)
            {
                return ApiResponse<AuthModel>.FailureResponse(
                    message: "Password reset failed.",
                    statusCode: 400,
                    errors: result.Errors.Select(e => e.Description).ToList()
                );
            }
            var authModel = new AuthModel
            {
                IsAuthenticated = true,
                //Message = "Password reset successfully!"
            };
            return ApiResponse<AuthModel>.SuccessResponse(
                data: authModel,
                message: "Password reset successfully.",
                statusCode: 200
            );
        }

        public async Task<ApiResponse<Profile>> GetProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return ApiResponse<Profile>.FailureResponse(
                    message:"User not found.",
                    statusCode:404,
                    errors: new List<string> { "User with the provided ID does not exist." } );
            }
            var profile = new Profile
            {
                //  UserId = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };
            return ApiResponse<Profile>.SuccessResponse(
                data: profile,
                message: "the Process was successful",
                statusCode: 200
            );
        }

        public async Task<ApiResponse<AuthModel>> UpdateProfileAsync(string userId, UpdateProfile model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
               return new ApiResponse<AuthModel>
                {
                    Success = false,
                    StatusCode = 404,
                    Message = "User not found.",
                    Errors = new List<string> { "User with the provided ID does not exist." }
                };
            }
            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;
            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
               return new ApiResponse<AuthModel>
                {
                    Success = false,
                    StatusCode = 500,
                    Message = "Profile update failed.",
                    Errors = result.Errors.Select(e => e.Description).ToList()
                };
            }
            authModel.IsAuthenticated = true;
            //authModel.Message = "Profile updated successfully!";
       return new ApiResponse<AuthModel>
            {
                Success = true,
                StatusCode = 200,
                Message = "Profile updated successfully.",
                Data = authModel
            };
        }

        #endregion
    }
}

