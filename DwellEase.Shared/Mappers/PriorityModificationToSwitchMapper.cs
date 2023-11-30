using DwellEase.Domain.Entity;
using DwellEase.Domain.Enum;
using DwellEase.Domain.Models.Requests;

namespace DwellEase.Shared.Mappers;

public class PriorityModificationToSwitchMapper
{
    public SwitchPriorityRequest MapToSwitchPriorityRequest(PriorityModificationRequest request)
    {
        return new SwitchPriorityRequest
        {
            Id = Guid.Empty, 
            NewType = MapToPriorityType(request.NewType),
            UserId = Guid.Parse(request.UserId),
            ApartmentPageId = Guid.Parse(request.ApartmentPageId),
            IsApproved = false 
        };
    }

    private PriorityType MapToPriorityType(string newType)
    {
        if (!Enum.IsDefined(typeof(PriorityType), newType))
        {
            throw new Exception("Invalid priority type");
        }
        return (PriorityType)Enum.Parse(typeof(PriorityType), newType);
    }
}