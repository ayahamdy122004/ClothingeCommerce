using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Account;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model.authonution;

namespace E_Commerce.services.AccountManager
{
    public interface IAccountManagerServices
    {
        Task<ApiResponse<string>> ForgotPasswordAsync(ForgotPassword model);
        Task<ApiResponse<AuthModel>> ResetPasswordAsync(ResetPassword model);
        Task<ApiResponse<Profile>> GetProfileAsync(string userId);
        Task<ApiResponse<AuthModel> > UpdateProfileAsync(string userId, UpdateProfile model);
    }
}
