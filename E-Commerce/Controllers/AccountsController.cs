using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.CUSTOMER;
using E_Commerce.Entities.DTO.Models.BRANDS;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model.authonution;
using E_Commerce.Helpers;
using E_Commerce.services.AccountManager;
using E_Commerce.services.AuthenticationServices;
using E_Commerce.services.CustomerServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AccountController : ControllerBase
    {
        #region
        private readonly IAccountManagerServices account;
        private readonly ICustomerService service;
        private readonly IAuthenticationservice auth;

        public AccountController(IAccountManagerServices account, ICustomerService service, IAuthenticationservice auth )
        {
            this.account = account;
            this.service = service;
            this.auth = auth;
        }
        #endregion


        #region customerProfile
        [HttpGet("displayCustomer/{Email}")]
        [ProducesResponseType(typeof(ApiResponse<UserProfileResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetCustomer(string Email)
        {
            var customer = await service.GetCustomer(Email);
            if (customer == null)
                return BadRequest(customer);
            return Ok(customer);

        }

        [HttpPut("UpdateProfile")]
        [ProducesResponseType(typeof(ApiResponse<UserProfileResponseDTO>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> UpdateCustomer([FromQuery] string email,[FromBody] UpdateUserProfileDTO model)
        {
          var updatedCustomer = await service.UpdateCustomer(email, model);

            if (updatedCustomer == null)
            {
                return NotFound(model);
            }

            return Ok(updatedCustomer);
        }


        #endregion

        #region   set and reset password
        [HttpPost("forgot-password")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword model)
        {
            var message = await account.ForgotPasswordAsync(model);
            return Ok(message);
        }


        [HttpPost("reset-password")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
        {
            var result = await auth.ResetPasswordAsync(model);
            if (!result.Success)
                BadRequest(result);

            return Ok(result);
        }
        #endregion
    }
}