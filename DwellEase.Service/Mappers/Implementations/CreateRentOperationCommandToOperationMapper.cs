using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;
using DwellEase.Service.Mappers.Interfaces;

namespace DwellEase.Service.Mappers.Implementations;

public class CreateRentOperationCommandToOperationMapper:IMapper<ApartmentOperation,CreateRentOperationCommand>
{
    public ApartmentOperation MapTo(CreateRentOperationCommand command)
    {
        return new ApartmentOperation()
        {
            Price = command.Price,
            ApartmentPageId = command.ApartmentPageId,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow + command.RentalPeriod,
            UserId = command.UserId,
            OperationType = command.OperationType
        };
    }
}