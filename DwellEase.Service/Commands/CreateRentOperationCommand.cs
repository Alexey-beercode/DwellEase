using DwellEase.Domain.Enum;
using DwellEase.Domain.Models.Requests;
using MediatR;

namespace DwellEase.Service.Commands;

public class CreateRentOperationCommand:IRequest<bool>
{
    public OperationType OperationType { get; set; } = OperationType.Rent;
    public Guid UserId { get; set; }
    public Guid ApartmentPageId { get; set; }
    public decimal Price { get; set; }
    public TimeSpan RentalPeriod { get; set; }
}