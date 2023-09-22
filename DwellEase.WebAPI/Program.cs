using System.Configuration;
using System.Text;
using DwellEase.WebAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using NLog;
using NLog.Web;
using LogLevel = NLog.LogLevel;
var builder = WebApplication.CreateBuilder(args);

builder.AddDatabase();
builder.AddServices();
builder.AddAuthentication();
builder.AddLogging();

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var app = builder.Build();
app.AddApplicationMiddleware();
app.AddSignalRConfiguration();
logger.Log(LogLevel.Info,"Program initial");
app.Run();