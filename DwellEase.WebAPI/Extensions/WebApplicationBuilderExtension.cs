using System.Text;
using DwellEase.DataManagement.DataSenders;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Entity;
using DwellEase.Service.Handlers;
using DwellEase.Service.Queries;
using DwellEase.Service.Services.Implementations;
using DwellEase.Service.Services.Interfaces;
using DwellEase.WebAPI.BackgroundTasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using NLog.Web;

namespace DwellEase.WebAPI.Extensions;

public static class WebApplicationBuilderExtension
{
    public static void AddServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ApartmentPageRepository>();
        builder.Services.AddScoped<ApartmentOperationRepository>();
        builder.Services.AddScoped<UserRoleRepository>();
        builder.Services.AddScoped<UserService>();
        builder.Services.AddScoped<RoleService>();
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<RoleRepository>();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<ApartmentPageService>();
        builder.Services.AddScoped<ApartmentOperationService>();
        builder.Services.AddScoped<RentalService>();
        builder.Services.AddScoped<IImageService,ImageService>();
        builder.Services.AddScoped<IImageService>();
        builder.Services.AddScoped<ImageService>();
        builder.Services.AddControllers();
        builder.Services.AddSignalR();
        builder.Services.AddHostedService<RentalExpirationWorker>();
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));
        builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(
            typeof(GetAllApartmentPagesQueryHandler).Assembly,
            typeof(GetAllApartmentPagesQuery).Assembly
        ));
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
        mongoDatabase.CreateCollectionAsync("ApartmentPages");
        mongoDatabase.CreateCollectionAsync("ApartmentOperations");
        mongoDatabase.CreateCollectionAsync("UserRole");
        builder.Services.AddSingleton(mongoDatabase);
        UserDataSeeder.SeedData();
        RoleDataSender.SeedData();
        UserRoleDataSender.SeedData();
    }

    public static void AddAuthentication(this WebApplicationBuilder builder)
    {
        var jwtSettings = builder.Configuration.GetSection("Jwt");
        
        var key = Encoding.UTF8.GetBytes(jwtSettings["Secret"]);
        var issuer = jwtSettings["Issuer"];
        var audience = jwtSettings["Audience"];

        builder.Services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false; 
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = issuer,
                ValidAudience = audience,
                IssuerSigningKey = new SymmetricSecurityKey(key),
            };
        });
        builder.Services.AddAuthorization(options => options.DefaultPolicy =
            new AuthorizationPolicyBuilder
                    (JwtBearerDefaults.AuthenticationScheme)
                .RequireAuthenticatedUser()
                .Build());
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("CreatorArea", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Creator");
            });

            options.AddPolicy("AdminArea", policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole("Admin");
            });
        });


    }

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
    }
}