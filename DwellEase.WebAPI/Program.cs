using DwellEase.WebAPI.Extensions;
using NLog;
using NLog.Web;
using LogLevel = NLog.LogLevel;
var builder = WebApplication.CreateBuilder(args);


//builder.AddIdentity();
builder.AddDatabase();
builder.AddServices();
builder.AddAuthentication();
builder.AddLogging();

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
var app = builder.Build();
app.AddApplicationMiddleware();
app.MapControllers();
app.AddSignalRConfiguration();
logger.Log(LogLevel.Info,"Program initial");
app.Run();