using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers;

public class CreateRentOperationCommandHandler:IRequestHandler<CreateRentOperationCommand, bool>
{
    private readonly ApartmentOperationService _apartmentOperationService;

    public CreateRentOperationCommandHandler(ILogger<CreateRentOperationCommandHandler> logger, ApartmentOperationService apartmentOperationService)
    {
        _apartmentOperationService = apartmentOperationService;
    }
    public async Task<bool> Handle(CreateRentOperationCommand request, CancellationToken cancellationToken)
    {
        var operation = new ApartmentOperation()
        {
            Price = request.Price,
            ApartmentPageId = request.ApartmentPageId,
            StartDate = DateTime.UtcNow,
            EndDate = DateTime.UtcNow + request.RentalPeriod,
            UserId = request.UserId,
            OperationType = request.OperationType
        };
        await _apartmentOperationService.CreateRentOperationAsync(operation);
        return true;
    }
}