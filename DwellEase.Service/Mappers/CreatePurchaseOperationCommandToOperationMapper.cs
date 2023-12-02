using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;

namespace DwellEase.Service.Mappers;

public class CreatePurchaseOperationCommandToOperationMapper
{
    public ApartmentOperation MapTo(CreatePurchaseOperationCommand command)
    {
        return new ApartmentOperation()
        {
            ApartmentPageId = command.ApartmentPageId,
            OperationType = command.OperationType,
            UserId = command.UserId,
            Price = command.Price
        };
    }
}