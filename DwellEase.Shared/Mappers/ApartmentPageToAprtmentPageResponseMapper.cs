using DwellEase.Domain.Entity;
using DwellEase.Domain.Models.Responses;
using Microsoft.AspNetCore.Mvc;

namespace DwellEase.Shared.Mappers;

public class ApartmentPageToAprtmentPageResponseMapper
{
    public ApartmentPageResponse MapTo(ApartmentPage page)
    {
        var images = new List<FileContentResult>();
        page.Images.ForEach(a=>images.Add(new FileContentResult(a.Data,a.ContentType)));

        return new ApartmentPageResponse()
        {
            Apartment = page.Apartment,
            ApprovalStatus = page.ApprovalStatus.ToString(),
            Date = page.Date,
            DaylyPrice = page.DaylyPrice,
            Price = page.Price,
            Id = page.Id,
            Images = images,
            IsAvailableForPurchase = page.IsAvailableForPurchase,
            OwnerId = page.OwnerId,
            PhoneNumber = page.PhoneNumber,
            PriorityType = page.PriorityType.ToString(),
            Status = page.Status.ToString(),
            Title = page.Title
        };
    }
}