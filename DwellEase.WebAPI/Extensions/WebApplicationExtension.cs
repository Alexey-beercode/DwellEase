﻿using DwellEase.WebAPI.Hubs;

namespace DwellEase.WebAPI.Extensions;

public static class WebApplicationExtension
{
    public static void AddApplicationMiddleware(this WebApplication app)
    {
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseCors(builder =>
        {
            builder.WithOrigins("https://localhost:44315") 
                .AllowAnyMethod()
                .AllowAnyHeader();
        }); 
        app.UseRouting();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapControllers();
    }
    public static void AddSignalRConfiguration(this WebApplication app)
    {
        app.MapHub<RentalHub>("/rentalhub");
        app.MapFallbackToFile("index.html");
    }

    public static void AddSwagger(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }
}