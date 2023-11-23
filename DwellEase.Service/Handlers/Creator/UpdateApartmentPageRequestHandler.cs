using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Queries.Creator;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers.Creator;

public class UpdateApartmentPageRequestHandler:IRequestHandler<UpdateApartmentPageRequest,bool>
{
    private readonly ILogger<UpdateApartmentPageRequestHandler> _logger;
    private readonly ApartmentPageService _apartmentPageService;

    public UpdateApartmentPageRequestHandler(ILogger<UpdateApartmentPageRequestHandler> logger, ApartmentPageService apartmentPageService)
    {
        _logger = logger;
        _apartmentPageService = apartmentPageService;
    }

    public async Task<bool> Handle(UpdateApartmentPageRequest request, CancellationToken cancellationToken)
    {
        if (!Guid.TryParse(request.PageId,out Guid guidId))
        { 
            throw new Exception("OwnerId is not valid");
        }

        var response = await _apartmentPageService.GetByIdAsync(guidId);
        if (response.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(response.Description);
        }

        var newApartmentPage = new ApartmentPage()
        {
            Apartment = response.Data.Apartment,
            ApprovalStatus = response.Data.ApprovalStatus,
            Date = response.Data.Date,
            DaylyPrice = request.DailyPrice,
            Price = request.Price,
            Title = request.Title,
            Id = response.Data.Id,
            OwnerId = response.Data.OwnerId,
            Images = response.Data.Images,
            IsAvailableForPurchase = request.IsAvailableForPurchase,
            Status = response.Data.Status,
            PriorityType = response.Data.PriorityType,
            PhoneNumber = new PhoneNumber(request.PhoneNumber)
        };
        await _apartmentPageService.EditAsync(newApartmentPage);
        return true;
    }
}