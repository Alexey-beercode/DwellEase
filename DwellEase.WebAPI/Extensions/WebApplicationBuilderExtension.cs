﻿using System.Text;
using DwellEase.DataManagement.DataSenders;
using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.DataManagement.Repositories.Interfaces;
using DwellEase.DataManagement.Stores;
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
        builder.Services.AddScoped<UserRepository>();
        builder.Services.AddScoped<RoleRepository>();
        builder.Services.AddScoped<IRoleStore<Role>, RoleStore>();
        builder.Services.AddScoped<IUserStore<User>, UserSrore>();
        builder.Services.AddScoped<ITokenService, TokenService>();
        builder.Services.AddScoped<TokenService>();
        builder.Services.AddScoped<ApartmentPageService>();
        builder.Services.AddScoped<ApartmentOperationService>();
        builder.Services.AddScoped<UserManager<User>>();
        builder.Services.AddScoped<RentalService>();
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
        builder.Services.AddSingleton(mongoDatabase);
        UserDataSeeder.SeedData();
        RoleDataSender.SeedData();
    }

    public static void AddIdentity(this WebApplicationBuilder builder)
    {
        builder.Services.AddIdentity<User, Role>(options =>
        {
            options.Password.RequiredLength = 5;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireUppercase = false;
            options.Password.RequireDigit = false;
        }).AddRoleStore<RoleStore>().AddUserStore<UserSrore>().AddDefaultTokenProviders();
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
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminArea", policy => policy.RequireRole("Admin"));
            options.AddPolicy("CreatorArea", policy => policy.RequireRole("Creator"));
        });

    }

    public static void AddLogging(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        builder.Host.UseNLog();
    }
}