//using ClothingStore.Entities;
//using E_Commerce.Entities.DTO; // تأكدي إن الـ Models هنا
//using E_Commerce.Entities.DTO.Idetity;
//using E_Commerce.Entities.DTO.ResponseAPIs;
//using E_Commerce.Entities.Model.authonution;
//using E_Commerce.Helpers;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.Options;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;

//namespace E_Commerce.services.AuthenticationServices
//{
//    public class AuthenticationService : IAuthenticationservice // تم تصحيح الاسم
//    {
//        #region
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly JWT _jwt;

//        public AuthenticationService(
//            UserManager<ApplicationUser> userManager,
//            RoleManager<IdentityRole> roleManager,
//            IOptions<JWT> jwt)
//        {
//            _userManager = userManager;
//            _roleManager = roleManager;
//            _jwt = jwt.Value;
//        }

//        #endregion

//        #region auth(login,register,addrole,generatetoken,confirmemail)

//        public async Task<ApiResponse<AuthModel>> ConfirmEmailAsync(ConfirmEmail model)
//        {

//            var authModel = new AuthModel();
//            var user = await _userManager.FindByIdAsync(model.UserId);
//            if (user == null)
//            {
//                return new ApiResponse<AuthModel>
//                {Success=false,
//                    Message = "User not found!",
//                    StatusCode = 404,
//                   Errors = new List<string> { "Email is Not Active" }

//                };
//            }
//            var result = await _userManager.ConfirmEmailAsync(user, model.Token);
//            if (!result.Succeeded)
//            {
//                return new ApiResponse<AuthModel>
//                {Success = false,
//                    Message = "Email confirmation failed!",
//                    StatusCode = 400,
//                    Errors = new List<string> { "Email is Not Active" }
//                };
//            }
//            authModel.IsAuthenticated = true;
//            //authModel.Message = "Email confirmed successfully!";
//            return new ApiResponse<AuthModel>
//            {
//                Message = "Email confirmed successfully!",
//                StatusCode = 200,
//                Data = authModel
//            };


//        }
//        public async Task<ApiResponse<AuthModel>> Register(RegisterModel model)
//        {
//            if (await _userManager.FindByEmailAsync(model.Email) is not null)
//            {
//                //  return new AuthModel { Message = "Email is already registered!" };
//                AuthModel m = new AuthModel()
//                {
//                    Message = "Email is already registered!",
//                    IsAuthenticated = false,
//                    Roles = new List<string>(),
//                    Username = null,
//                    Email = model.Email,
//                   Token = null
//                };
//                return new ApiResponse<AuthModel>
//                {
//                    Message = "Email is already registered!",
//                    StatusCode = 400,
//                   // Errors = new List<string>{ "Email is already registered!" }

//                };
//            }

//            var user = new ApplicationUser
//            {
//                UserName = model.Email,
//                Email = model.Email,
//                FirstName = model.FirstName,
//                LastName = model.LastName,
//                PhoneNumber = model.PhoneNumber
//            };

//            var result = await _userManager.CreateAsync(user, model.Password);

//            if (!result.Succeeded)
//            {
//                var errors = string.Empty;
//                foreach (var error in result.Errors)
//                    errors += $"{error.Description}, ";

//             return new ApiResponse<AuthModel>
//                {Success=false,
//                    Message = "User registration failed!",
//                    StatusCode = 400,
//                    Errors = new List<string> { errors.TrimEnd(',', ' ') }
//                };
//            }

//            await _userManager.AddToRoleAsync(user, "Customer");


//            var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

//             new AuthModel
//            {
//                //Message = "User registered successfully! Please confirm your email.",
//                Email = user.Email,
//                IsAuthenticated = true,
//                Roles = new List<string> { "Customer" },
//                Username = user.Email,
//                // 2. نحط التوكن ده في مكان الـ Token عشان يتشاف في السوابجر للـ Dev
//                Token = emailConfirmToken
//            };
//            return new ApiResponse<AuthModel>
//            {Success=true,
//                Message = "User registered successfully! Please confirm your email.",
//                StatusCode = 200,
//                Data = new AuthModel
//                {
//                    //Message = "User registered successfully! Please confirm your email.",
//                    Email = user.Email,
//                    IsAuthenticated = true,
//                    Roles = new List<string> { "Customer" },
//                    Username = user.Email,
//                    Token = emailConfirmToken
//                }
//            };
//            // ===============================================
//        }

//        //public async Task<ApiResponse<AuthModel>> Login(LoginModel model)
//        //{
//        //    var authModel = new AuthModel();

//        //    var user = await _userManager.FindByEmailAsync(model.Email);

//        //    if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
//        //    {
//        //        //authModel.Message = "Email or Password is incorrect!";
//        //        return  new ApiResponse<AuthModel>
//        //        {Success = false,
//        //            Message = "Email or Password is incorrect!",
//        //            StatusCode = 400,
//        //            Errors = new List<string> { "Email or Password is incorrect!" }
//        //        };
//        //    }
//        //    if (!user.EmailConfirmed)
//        //    {
//        //        //authModel.Message = "Email is not confirmed!";
//        //        return new ApiResponse<AuthModel>
//        //        {Success = false,
//        //            Message = "Email is not confirmed!",
//        //            StatusCode = 400,
//        //            Errors = new List<string> { "Email is not confirmed!" }
//        //        };
//        //    }

//        //    var jwtSecurityToken = await CreateJwtToken(user);
//        //    var rolesList = await _userManager.GetRolesAsync(user);

//        //    authModel.IsAuthenticated = true;
//        //    authModel.Token = new JwtSecurityTokenHandler().WriteToken(user);
//        //    authModel.Email = user.Email;
//        //    authModel.Username = user.UserName;
//        //    authModel.ExpiresOn = DateTime.UtcNow;
//        //    authModel.Roles = rolesList.ToList();

//        //    return new ApiResponse<AuthModel>
//        //    {
//        //        Message = "Login successful!",
//        //        StatusCode = 200,
//        //        Data = authModel
//        //    };
//        //}
//        public async Task<ApiResponse<AuthModel>> Login(LoginModel model)
//        {
//            var user = await _userManager.FindByEmailAsync(model.Email);

//            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
//            {
//                return ApiResponse<AuthModel>.FailureResponse(
//                    message: "Email or Password is incorrect!",
//                    statusCode: 400,
//                    errors: new List<string> { "Email or Password is incorrect!" }
//                );
//            }

//            if (!user.EmailConfirmed)
//            {
//                return ApiResponse<AuthModel>.FailureResponse(
//                    message: "Email is not confirmed!",
//                    statusCode: 400,
//                    errors: new List<string> { "Please confirm your email before logging in." }
//                );
//            }

//            // توليد التوكن كـ JwtSecurityToken مباشر
//            var jwtSecurityToken = await CreateJwtToken(user);
//            var rolesList = await _userManager.GetRolesAsync(user);

//            var authModel = new AuthModel
//            {
//                IsAuthenticated = true,
//                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
//                Email = user.Email,
//                Username = user.UserName,
//                ExpiresOn = jwtSecurityToken.ValidTo, // ضبط تاريخ الانتهاء الصحيح للتوكن
//                Roles = rolesList.ToList()
//            };

//            return ApiResponse<AuthModel>.SuccessResponse(
//                data: authModel,
//                message: "Login successful!",
//                statusCode: 200
//            );
//        }
//        private async Task<ApiResponse<JwtSecurityToken>> CreateJwtToken(ApplicationUser user)
//        {
//            var userClaims = await _userManager.GetClaimsAsync(user);
//            var roles = await _userManager.GetRolesAsync(user);
//            var roleClaims = new List<Claim>();

//            foreach (var role in roles)
//                roleClaims.Add(new Claim("roles", role));

//            var claims = new[]
//            {
//                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
//                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
//                new Claim(JwtRegisteredClaimNames.Email, user.Email),
//                new Claim("uid", user.Id)
//            }
//            .Union(userClaims)
//            .Union(roleClaims);

//            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
//            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

//            var jwtSecurityToken = new JwtSecurityToken(
//                issuer: _jwt.Issuer,
//                audience: _jwt.Audience,
//                claims: claims,
//                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
//                signingCredentials: signingCredentials);

//            return new ApiResponse<JwtSecurityToken>
//            {Success=true,
//                Message = "JWT token created successfully!",
//                StatusCode = 200,
//                Data = jwtSecurityToken
//            };
//        }
//        public async Task<string> AddRole(AddRoleModel model)
//        {
//            var user = await _userManager.FindByIdAsync(model.UserId);

//            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
//             //   return "Invalid user ID or Role";
//             return ApiResponse<string>.FailureResponse(

//                    message: "Invalid user ID or Role",
//                    statusCode: 400,

//                    errors: new List<string> { "Invalid user ID or Role" }
//                ).Message;

//            if (await _userManager.IsInRoleAsync(user, model.Role))
//            {
//                return new ApiResponse<string>
//                {
//                    Message = "User already has this role.",
//                    StatusCode = 400,
//                    Errors = new List<string> { "User already has this role." }
//                }.Message;
//            }


//            var result = await _userManager.AddToRoleAsync(user, model.Role);

//            if(result.Succeeded)
//            {
//                return new ApiResponse<string>
//                {Success = true,
//                    Message = "Role added successfully.",
//                    StatusCode = 200
//                }.Message;
//            }
//            else
//            {
//                return new ApiResponse<string>
//                {Success = false,
//                    Message = "Failed to add role.",
//                    StatusCode = 400,
//                    Errors = result.Errors.Select(e => e.Description).ToList()
//                }.Message;
//            }

//        }
//        #endregion

//    }
//}

using ClothingStore.Entities;
using E_Commerce.Entities.DTO;
using E_Commerce.Entities.DTO.Idetity;
using E_Commerce.Entities.DTO.ResponseAPIs;
using E_Commerce.Entities.Model.authonution;
using E_Commerce.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace E_Commerce.services.AuthenticationServices
{
    public class AuthenticationService : IAuthenticationservice
    {
        #region Constructor & Fields
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthenticationService(
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWT> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }
        #endregion

        #region Authentication Methods

        public async Task<ApiResponse<AuthModel>> ConfirmEmailAsync(ConfirmEmail model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);
            if (user == null)
            {
                return ApiResponse<AuthModel>.FailureResponse(
                    message: "User not found!",
                    statusCode: 404,
                    errors: new List<string> { "User with the provided ID does not exist." }
                );
            }

            var result = await _userManager.ConfirmEmailAsync(user, model.Token);
            if (!result.Succeeded)
            {
                return ApiResponse<AuthModel>.FailureResponse(
                    message: "Email confirmation failed!",
                    statusCode: 400,
                    errors: result.Errors.Select(e => e.Description).ToList()
                );
            }

            var authModel = new AuthModel
            {
                IsAuthenticated = true,
                Email = user.Email,
                Username = user.UserName
            };

            return ApiResponse<AuthModel>.SuccessResponse(
                data: authModel,
                message: "Email confirmed successfully!",
                statusCode: 200
            );
        }

        public async Task<ApiResponse<AuthModel>> Register(RegisterModel model)
        {
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                return ApiResponse<AuthModel>.FailureResponse(
                    message: "Email is already registered!",
                    statusCode: 400,
                    errors: new List<string> { "Email is already registered!" }
                );
            }

            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PhoneNumber = model.PhoneNumber
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return ApiResponse<AuthModel>.FailureResponse(
                    message: "User registration failed!",
                    statusCode: 400,
                    errors: result.Errors.Select(e => e.Description).ToList()
                );
            }

            await _userManager.AddToRoleAsync(user, "Customer");

            var emailConfirmToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            var authModel = new AuthModel
            {
                Email = user.Email,
                IsAuthenticated = true,
                Roles = new List<string> { "Customer" },
                Username = user.Email,
                Token = emailConfirmToken
            };

            return ApiResponse<AuthModel>.SuccessResponse(
                data: authModel,
                message: "User registered successfully! Please confirm your email.",
                statusCode: 200
            );
        }

        public async Task<ApiResponse<AuthModel>> Login(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return ApiResponse<AuthModel>.FailureResponse(
                    message: "Email or Password is incorrect!",
                    statusCode: 400,
                    errors: new List<string> { "Email or Password is incorrect!" }
                );
            }

            if (!user.EmailConfirmed)
            {
                return ApiResponse<AuthModel>.FailureResponse(
                    message: "Email is not confirmed!",
                    statusCode: 400,
                    errors: new List<string> { "Please confirm your email before logging in." }
                );
            }
            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            var authModel = new AuthModel
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Email = user.Email,
                Username = user.UserName,
                ExpiresOn = jwtSecurityToken.ValidTo,
                Roles = rolesList.ToList()
            };

            return ApiResponse<AuthModel>.SuccessResponse(
                data: authModel,
                message: "Login successful!",
                statusCode: 200
            );
        }

        public async Task<ApiResponse<string>> AddRole(AddRoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.Role))
            {
                return ApiResponse<string>.FailureResponse(
                    message: "Invalid user ID or Role",
                    statusCode: 400,
                    errors: new List<string> { "Invalid user ID or Role" }
                );
            }

            if (await _userManager.IsInRoleAsync(user, model.Role))
            {
                return ApiResponse<string>.FailureResponse(
                    message: "User already has this role.",
                    statusCode: 400,
                    errors: new List<string> { "User already has this role." }
                );
            }

            var result = await _userManager.AddToRoleAsync(user, model.Role);

            if (result.Succeeded)
            {
                return ApiResponse<string>.SuccessResponse(
                    data: model.Role,
                    message: "Role added successfully.",
                    statusCode: 200
                );
            }

            return ApiResponse<string>.FailureResponse(
                message: "Failed to add role.",
                statusCode: 400,
                errors: result.Errors.Select(e => e.Description).ToList()
            );
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            return new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);
        }

        #endregion
    }
}