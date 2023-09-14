using DwellEase.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace DwellEase.WebAPI.Hubs;

public class RentalHub : Hub
{
    public async Task SendRemainingTimeUpdate(ApartmentPageRentInfo apartmentPageRentInfo)
    {
        await Clients.User(apartmentPageRentInfo.UserId.ToString()).SendAsync("ReceiveRemainingTimeUpdate", apartmentPageRentInfo);
    }
}