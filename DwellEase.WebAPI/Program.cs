using DwellEase.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.AddIdentity();
builder.AddDatabase();
builder.AddServices();
builder.AddAuthentication();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();
app.AddApplicationMiddleware();
app.MapControllers();
app.Run();