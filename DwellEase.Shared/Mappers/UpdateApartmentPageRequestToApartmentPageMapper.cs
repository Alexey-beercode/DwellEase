using DwellEase.Domain.Entity;
using DwellEase.Domain.Models;
using DwellEase.Domain.Models.Requests;
using DwellEase.Domain.Models.Responses;

namespace DwellEase.Shared.Mappers;

public class UpdateApartmentPageRequestToApartmentPageMapper
{
    public ApartmentPage MapTo(UpdateApartmentPageRequest request,BaseResponse<ApartmentPage> response)
    {
        return new ApartmentPage()
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
    }
}