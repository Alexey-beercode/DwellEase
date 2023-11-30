using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;

namespace DwellEase.Shared.Mappers;

public class CreatePageRequestToApartmentPageMapper
{
    public ApartmentPage MapTo(CreateApartmentPageRequest request,List<Image> images,Guid userId, ApartmentType type)
    {
        return new ApartmentPage()
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
                ApartmentType = type,
                Area = request.Area,
                Rooms = request.Rooms,
                
            },
            Title = request.Title,
            DaylyPrice = request.DailyPrice,
            Price = request.Price,
            Images = images,
            IsAvailableForPurchase = request.IsAvailableForPurchase,
            OwnerId = userId,
            PhoneNumber = new PhoneNumber(request.PhoneNumber)
        };
    }
}