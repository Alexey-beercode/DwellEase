using DwellEase.WebAPI.Hubs;

namespace DwellEase.WebAPI.Extensions;

public static class WebApplicationExtension
{
    public static void AddApplicationMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
    }
    public static void AddSignalRConfiguration(this WebApplication app)
    {
        app.MapHub<RentalHub>("/rentalhub");
        app.MapFallbackToFile("index.html");
    }
}