using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Idetity;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model.authonution;
using Microsoft.AspNetCore.Identity.Data;

namespace E_Commerce.services.AuthenticationServices
{
    public interface IAuthenticationservice
    {
        public Task<ApiResponse<AuthModel>> Register(RegisterModel model);
        public Task<ApiResponse<AuthModel>> Login(LoginModel model);
        public Task<ApiResponse<string>> AddRole(AddRoleModel model);
        Task<ApiResponse<AuthModel>> ConfirmEmailAsync(ConfirmEmail model);
   
    }
}
