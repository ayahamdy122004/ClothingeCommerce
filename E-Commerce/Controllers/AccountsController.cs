using E_Commerce.Entities.DTO;
using E_Commerce.services.AccountManager;
using E_Commerce.services.AuthenticationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // محدش يدخل من غير Token
    public class AccountController : ControllerBase
    {
        private readonly IAccountManagerServices  account;

        public AccountController(IAccountManagerServices account)
        {
           this.account = account;
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            // ⚠️ هنا بنجيب الـ ID من الـ Token اللي أرسله العميل (أمان وليس من الـ Body)
            var userId = User.FindFirstValue("uid"); // نفس الـ Claim اللي حطيناه لما عملنا الـ JWT

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Invalid token." });

            var profile = await account.GetProfileAsync(userId);
            if (profile == null)
                return NotFound(new { message = "User not found." });

            return Ok(profile);
        }

        [HttpPut("profile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfile model)
        {
            var userId = User.FindFirstValue("uid");

            if (string.IsNullOrEmpty(userId))
                return Unauthorized(new { message = "Invalid token." });

            var result = await account.UpdateProfileAsync(userId, model);
            if (!result.IsAuthenticated)
                return BadRequest(result);

            return Ok(result);
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