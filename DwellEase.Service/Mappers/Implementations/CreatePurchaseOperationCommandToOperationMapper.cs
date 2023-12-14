using DwellEase.DataManagement.Repositories.Implementations;
using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;
using DwellEase.Service.Mappers.Interfaces;

namespace DwellEase.Service.Mappers.Implementations;

public class CreatePurchaseOperationCommandToOperationMapper:IMapper<ApartmentOperation,CreatePurchaseOperationCommand>
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