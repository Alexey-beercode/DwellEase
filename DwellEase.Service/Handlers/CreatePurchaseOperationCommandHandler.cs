using DwellEase.Service.Commands;
using DwellEase.Service.Services.Implementations;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DwellEase.Service.Handlers;

public class CreatePurchaseOperationCommandHandler:IRequestHandler<CreatePurchaseOperationCommand, Guid>
{
    private readonly ILogger<CreatePurchaseOperationCommandHandler> _logger;
    private readonly ApartmentOperationService _apartmentOperationService;

    public CreatePurchaseOperationCommandHandler(ILogger<CreatePurchaseOperationCommandHandler> logger, ApartmentOperationService apartmentOperationService)
    {
        _logger = logger;
        _apartmentOperationService = apartmentOperationService;
    }

    public Task<Guid> Handle(CreatePurchaseOperationCommand request, CancellationToken cancellationToken)
    {
        
    }
    
}