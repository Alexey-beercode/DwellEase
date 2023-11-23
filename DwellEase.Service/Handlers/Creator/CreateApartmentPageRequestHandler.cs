using System.Net;
using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using DwellEase.Service.Services.Implementations;
using DwellEase.Service.Services.Interfaces;
using MediatR;

namespace DwellEase.Service.Handlers.Creator;

public class CreateApartmentPageRequestHandler:IRequestHandler<CreateApartmentPageRequest,bool>
{
    private readonly ApartmentPageService _apartmentPageService;
    private readonly IImageService _imageService;

    public CreateApartmentPageRequestHandler(IImageService imageService, ApartmentPageService apartmentPageService)
    {
        _imageService = imageService;
        _apartmentPageService = apartmentPageService;
    }

    public async Task<bool> Handle(CreateApartmentPageRequest request, CancellationToken cancellationToken)
    {
        var imageServiceResponse = _imageService.UploadImage(request.Images);
        if (imageServiceResponse.StatusCode!=HttpStatusCode.OK)
        {
            throw new Exception(imageServiceResponse.Description);
        }

        if (!Guid.TryParse(request.OwnerId,out Guid ownerGuidId))
        {
            throw new ("OwnerId is not valid");
        }
        if (!ApartmentType.TryParse(request.ApartmentType, out ApartmentType apartmentType))
        {
            throw new Exception("Apartment type is not valid");
        }

        ApartmentPage apartmentPage = new ApartmentPage()
        {
            Apartment = new Apartment()
            {
                Address = new Address()
                {
                    Building = request.Building,
                    City = request.City,
                    HouseNumber = request.HouseNumber,
                    Street = request.Street
                },
                ApartmentType = apartmentType,
                Area = request.Area,
                Rooms = request.Rooms,
                
            },
            Title = request.Title,
            DaylyPrice = request.DailyPrice,
            Price = request.Price,
            Images = imageServiceResponse.Data,
            IsAvailableForPurchase = request.IsAvailableForPurchase,
            OwnerId = ownerGuidId,
            PhoneNumber = new PhoneNumber(request.PhoneNumber)
        };
        await _apartmentPageService.CreateAsync(apartmentPage);
        return true;
    }
}