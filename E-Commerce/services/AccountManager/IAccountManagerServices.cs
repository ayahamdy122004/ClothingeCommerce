using E_Commerce.Entities.DTO;
using E_Commerce.Entities.Model.authonution;

namespace E_Commerce.services.AccountManager
{
    public interface IAccountManagerServices
    {
        Task<string> ForgotPasswordAsync(ForgotPassword model);
        Task<AuthModel> ResetPasswordAsync(ResetPassword model);
        Task<Profile> GetProfileAsync(string userId);
        Task<AuthModel> UpdateProfileAsync(string userId, UpdateProfile model);
    }
}
