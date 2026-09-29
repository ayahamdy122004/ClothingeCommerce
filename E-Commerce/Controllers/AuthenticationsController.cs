using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Idetity;
using E_Commerce.Entities.DTO.Models.PRODUCTS;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model.authonution;
using E_Commerce.Helpers;
using E_Commerce.services.AuthenticationServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
      //  [Authorize(Roles = Role.Administrator)]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        [Produces("application/json")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.Register(model);
            if (!result.Success)
            {
                return BadRequest(result);
            }


            return Ok(result);
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.Login(model);


            if (!result.Success)
            {
                return NotFound(result);
            }


            return Ok(result);
        }

        [HttpPost("addrole")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> AddRole([FromBody] AddRoleModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await authService.AddRole(model);
if(!result.Success)
                NotFound(result);


            return Ok(model);
        }
        #endregion

        [HttpPost("confirm-email")]
        [ProducesResponseType(typeof(ApiResponse<AuthModel>), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> ConfirmEmail([FromBody] ConfirmEmail model)
            {
                var result = await authService.ConfirmEmailAsync(model);
               if(!result.Success)
                return BadRequest(result);  

                return Ok(result);
            }

         

        }
}
