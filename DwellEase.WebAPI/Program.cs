using DwellEase.WebAPI.Extensions;
using NLog;
using NLog.Web;
using LogLevel = NLog.LogLevel;
var builder = WebApplication.CreateBuilder(args);

builder.AddDatabase();
builder.AddServices();
builder.AddAuthentication();
builder.AddLogging();
builder.AddSwaggerDocumentation();
builder.AddMediatRHandlers();

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var app = builder.Build();
app.AddApplicationMiddleware();
app.AddSignalRConfiguration();
app.AddSwagger();
logger.Log(LogLevel.Error,"Program initial");
app.Run();