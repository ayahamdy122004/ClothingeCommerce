using E_Commerce.Entities.DTO;
using E_Commerce.Entities.Model.authonution;
using Microsoft.AspNetCore.Identity.Data;

namespace E_Commerce.services.AuthenticationServices
{
    public interface IAuthenticationservice
    {
        public Task<AuthModel> Register(RegisterModel model);
        public Task<AuthModel> Login(LoginModel model);
        public Task<string> AddRole(AddRoleModel model);
        Task<AuthModel> ConfirmEmailAsync(ConfirmEmail model);
   
    }
}
