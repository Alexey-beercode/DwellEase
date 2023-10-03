﻿using DwellEase.Domain.Models;
using Microsoft.AspNetCore.SignalR;

namespace DwellEase.WebAPI.Hubs;

public class RentalHub : Hub
{
    public async Task SendRemainingTimeUpdate(ApartmentPageRentResponse apartmentPageRentResponse)
    {
        await Clients.User(apartmentPageRentResponse.UserId.ToString()).SendAsync("ReceiveRemainingTimeUpdate", apartmentPageRentResponse);
    }
}