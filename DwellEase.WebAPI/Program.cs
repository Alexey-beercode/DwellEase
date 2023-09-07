using DwellEase.WebAPI.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.AddIdentity();
builder.AddDatabase();
builder.AddServices();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(Program).Assembly));

var app = builder.Build();
//app.AddApplicationMiddleware();
app.MapGet("/", () => "Hello World!");
app.Run();