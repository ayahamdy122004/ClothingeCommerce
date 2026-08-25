using ClothingStore.Entities;
using E_Commerce.Entities;
using E_Commerce.Entities.Data;
using E_Commerce.Helpers;
using E_Commerce.Repositories;
using E_Commerce.Repositories.Interfaces;
using E_Commerce.Repositorys.ProductRepo;
using E_Commerce.Repositorys.VariationRepo;
using E_Commerce.services.AccountManager;
using E_Commerce.services.AuthenticationServices;
using E_Commerce.services.ProductServices;
using E_Commerce.services.VariationProductServices;
using E_Commerce.Services;
using E_Commerce.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace E_Commerce
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 1. Add services to the container.
            builder.Services.AddControllers();

            // 2. Swagger Configuration (مع إعداد الـ JWT في السوابجر)
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Clothing E-Commerce API", Version = "v1" });

                // ده بيحط زر "Authorize" في السوابجر عشان تدخلي التوكن
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter a valid token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });
                options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                        },
                        new string[] { }
                    }
                });
            });


            // 3. Database Context
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            // 4. ASP.NET Core Identity
            builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                // تقدري تضيفي إعدادات الباسورد هنا لو حبيتي (Password Rules)
                options.Password.RequiredLength = 8;
                options.Password.RequireDigit = true;
                options.Password.RequireUppercase = true;
            })
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();


            // 5. JWT Configuration (أهم سطرين لشغل الـ [Authorize])
            builder.Services.Configure<JWT>(builder.Configuration.GetSection("JWT"));

            var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWT>();
            var key = Encoding.UTF8.GetBytes(jwtSettings.Key);

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false; // في التطوير بس
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });


            // 6. Repositories (Data Access Layer)
            builder.Services.AddScoped<IBrandRepository, BrandRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IVariationRepository, VariationRepository>();

            // 7. Services (Business Logic Layer)
            builder.Services.AddScoped<IAuthenticationservice, AuthenticationService>();
            builder.Services.AddScoped<IAccountManagerServices, AccountManagerServices>();
            builder.Services.AddScoped<IBrandService, BrandService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IVariationProductService, VariationProductService>();


            // 8. Build Application
            var app = builder.Build();

            // === Seed Roles (لو مش حاططاه في مكان تاني، خليه هنا) ===
            using (var scope = app.Services.CreateScope())
            {
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                string[] roles = { "Customer", "Administrator" };
                foreach (var role in roles)
                {
                    if (!await roleManager.RoleExistsAsync(role))
                        await roleManager.CreateAsync(new IdentityRole(role));
                }
            }
            // =================================================


            // 9. Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // الترتيب ده مهم جداً: Authentication قبل Authorization
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}