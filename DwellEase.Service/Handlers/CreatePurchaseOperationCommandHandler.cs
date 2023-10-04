using DwellEase.Domain.Entity;
using DwellEase.Service.Commands;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers;

public class CreatePurchaseOperationCommandHandler:IRequestHandler<CreatePurchaseOperationCommand, bool>
{
    private readonly ApartmentOperationService _apartmentOperationService;

    public CreatePurchaseOperationCommandHandler(ILogger<CreatePurchaseOperationCommandHandler> logger, ApartmentOperationService apartmentOperationService)
    {
        _apartmentOperationService = apartmentOperationService;
    }

    public async Task<bool> Handle(CreatePurchaseOperationCommand request, CancellationToken cancellationToken)
    {
        var operation = new ApartmentOperation()
        {
            ApartmentPageId = request.ApartmentPageId,
            OperationType = request.OperationType,
            UserId = request.UserId,
            Price = request.Price
        };
        await _apartmentOperationService.CreateAsync(operation);
        return true;
    }
    
}