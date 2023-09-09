using System.Text;
using DwellEase.DataManagement;
using DwellEase.DataManagement.DataSenders;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.DataManagement.Repositories.Interfaces;
using DwellEase.DataManagement.Repositories.Stores;
using DwellEase.DataManagement.Stores;
using DwellEase.Domain.Entity;
using DwellEase.Service.Services.Implementations;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace DwellEase.WebAPI.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IBaseRepository<ApartmentPage>, ApartmentPageRepository>();
        builder.Services.AddScoped<IRoleStore<Role>, RoleStore>();
        builder.Services.AddScoped<IUserStore<User>, UserSrore>();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddControllers();
    }

    public static void AddDatabase(this WebApplicationBuilder builder)
    {
        var mongoSettings = builder.Configuration.GetSection("MongoDbSettings");
        var mongoConnectionString = mongoSettings["ConnectionString"];
        var mongoDatabaseName = mongoSettings["DatabaseName"];

        var mongoClient = new MongoClient(mongoConnectionString);
        var mongoDatabase = mongoClient.GetDatabase(mongoDatabaseName);
        mongoDatabase.CreateCollectionAsync("Users");
        mongoDatabase.CreateCollectionAsync("Roles");
        mongoDatabase.CreateCollectionAsync("ApartmentPages", new CreateCollectionOptions()
        {
            
        });
        builder.Services.AddSingleton(mongoDatabase);
        UserDataSeeder.SeedData();
        RoleDataSender.SeedData();
    }

    public static void AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.User.RequireUniqueEmail = false;
            options.Password.RequiredLength = 5;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
        }).AddDefaultTokenProviders();
    }

    public static void AddAuthentication(this WebApplicationBuilder builder)
    {
        builder.Services.AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JwtSettings:Issuer"]!,
                    ValidAudience = builder.Configuration["JwtSettings:Audience"]!,
                    IssuerSigningKey =
                        new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Secret"]!))
                };
            });
        builder.Services.AddAuthorization(options => options.DefaultPolicy =
            new AuthorizationPolicyBuilder
                    (JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
    }
}