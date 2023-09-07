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
}