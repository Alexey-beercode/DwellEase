using System.Net;
using DwellEase.Domain.Models;
using DwellEase.Service.Services.Implementations;
using DwellEase.WebAPI.Hubs;
using Microsoft.AspNetCore.SignalR;

namespace DwellEase.WebAPI.BackgroundTasks;

public class RentalExpirationWorker : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<RentalExpirationWorker> _logger;
    private readonly IHubContext<RentalHub> _hubContext;

    public RentalExpirationWorker(IServiceProvider serviceProvider, ILogger<RentalExpirationWorker> logger, IHubContext<RentalHub> hubContext)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _hubContext = hubContext;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using (var scope = _serviceProvider.CreateScope())
                {
                    var rentalService = scope.ServiceProvider.GetRequiredService<RentalService>();
                    var response = await rentalService.CheckAndUpdateApartmentStatus();
                    if (response.StatusCode==HttpStatusCode.OK)
                    {
                        foreach (var rentInfo in response.Data )
                        {
                            await SendRemainingTimeUpdateToUser(rentInfo);
                        }
                    }
                  
                }

                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during rental expiration check.");
            }
        }
    }
    private async Task SendRemainingTimeUpdateToUser(ApartmentPageRentInfo rentInfo)
    {
        await _hubContext.Clients.User(rentInfo.UserId.ToString()).SendAsync("ReceiveRemainingTimeUpdate",rentInfo);
    }
}