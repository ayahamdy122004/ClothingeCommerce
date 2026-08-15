using E_Commerce.Entities.DTO;
using E_Commerce.services.AuthenticationServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationsController : ControllerBase
    {
        private readonly IAuthenticationservice authService;
        public AuthenticationsController(IAuthenticationservice authService)
        {
            this.authService = authService;
        }
        #region auth(login,register,addrole,generatetokenendpoint)
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.Register(model);

            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.Login(model);


            if (!result.IsAuthenticated)
                return BadRequest(result.Message);

            return Ok(result);
        }

        [HttpPost("addrole")]
        public async Task<IActionResult> AddRole([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.AddRole(model);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok(model);
        }
        #endregion

        [HttpPost("confirm-email")]
            public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmail model)
            {
                var result = await authService.ConfirmEmailAsync(model);
                if (!result.IsAuthenticated)
                    return BadRequest(result);

                return Ok(result);
            }

         

        }
}
