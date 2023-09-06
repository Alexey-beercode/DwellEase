using DwellEase.WebAPI.Extensions;
using SharedLibrary.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.AddIdentity();
builder.AddDatabase();
builder.AddServices();

var app = builder.Build();
//app.AddApplicationMiddleware();
app.MapGet("/", () => "Hello World!");
app.Run();