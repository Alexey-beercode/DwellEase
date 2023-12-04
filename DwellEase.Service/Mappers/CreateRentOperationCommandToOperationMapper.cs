using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;

namespace DwellEase.Service.Mappers;

public class CreateRentOperationCommandToOperationMapper
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