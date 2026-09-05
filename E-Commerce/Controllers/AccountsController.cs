using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.CUSTOMER;
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
        private readonly IAccountManagerServices  account;
        private readonly ICustomerService service;

        public AccountController(IAccountManagerServices account, ICustomerService service)
        {
           this.account = account;
        this.service = service;
        }
        [HttpGet("diplayCustomer({Email})")]
        public async Task<IActionResult> GetCustomer(string Email)
        { 
            var customer = await service.GetCustomer(Email);
            if (customer == null)
                return NotFound(new { message = "Customer not found." });
            return Ok(customer);

        }
 
        [HttpPut("UpdateProfile")] 
        public async Task<IActionResult> UpdateCustomer([FromQuery] string email, [FromBody] UpdateUserProfileDTO model)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return BadRequest(new { message = "Email is required." });
            }

            var updatedCustomer = await service.UpdateCustomer(email, model);

            if (updatedCustomer == null)
            {
                return NotFound(new { message = "Customer not found or update failed." });
            }

            return Ok(updatedCustomer);
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPassword model)
        {
            var message = await account.ForgotPasswordAsync(model);
            return Ok(new { message });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPassword model)
        {
            var result = await account.ResetPasswordAsync(model);
            if (!result.IsAuthenticated)
                return BadRequest(result);

            return Ok(result);
        }
    }
}